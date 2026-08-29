let device = null;
let outEndpoint = null;
let interfaceNumber = 0;

export async function conectar() {
    device = await navigator.usb.requestDevice({
        filters: [{ vendorId: 0x0A5F }]
    });

    try {
        await device.open();

        if (device.configuration === null) {
            await device.selectConfiguration(1);
        }

        const interfaces = device.configuration.interfaces;
        console.log('Total de interfaces:', interfaces.length);

        for (const iface of interfaces) {
            const alt = iface.alternate;
            console.log(`Interface ${iface.interfaceNumber}: classe=${alt.interfaceClass} subclasse=${alt.interfaceSubclass} protocolo=${alt.interfaceProtocol}`);
            for (const ep of alt.endpoints) {
                console.log(`  Endpoint ${ep.endpointNumber}: direcao=${ep.direction} tipo=${ep.type} tamanhoPacote=${ep.packetSize}`);
            }
        }

        let escolhida = interfaces.find(i => i.alternate.interfaceClass === 7);
        if (!escolhida) {
            escolhida = interfaces.find(i => i.alternate.endpoints.some(e => e.direction === 'out' && e.type === 'bulk'));
        }

        interfaceNumber = escolhida.interfaceNumber;
        const saida = escolhida.alternate.endpoints.find(e => e.direction === 'out' && e.type === 'bulk');
        outEndpoint = saida.endpointNumber;

        console.log('Interface escolhida:', interfaceNumber, 'Endpoint:', outEndpoint);

        await device.claimInterface(interfaceNumber);

        return device.productName;
    } catch (erro) {
        device = null;
        throw erro;
    }
}

export async function imprimir(zpl) {
    if (!device) {
        throw new Error('Impressora não conectada.');
    }

    const bytes = new TextEncoder().encode(zpl);
    const resultado = await device.transferOut(outEndpoint, bytes);
    console.log('Resultado do envio:', resultado.status, 'bytes enviados:', resultado.bytesWritten);
}

export function estaConectado() {
    return device !== null;
}
