let device = null;
let outEndpoint = null;
let interfaceNumber = 0;

function escolherInterface() {
    const interfaces = device.configuration.interfaces;
    let escolhida = interfaces.find(i => i.alternate.interfaceClass === 7);
    if (!escolhida) {
        escolhida = interfaces.find(i => i.alternate.endpoints.some(e => e.direction === 'out' && e.type === 'bulk'));
    }
    interfaceNumber = escolhida.interfaceNumber;
    const saida = escolhida.alternate.endpoints.find(e => e.direction === 'out' && e.type === 'bulk');
    outEndpoint = saida.endpointNumber;
}

export async function conectar() {
    device = await navigator.usb.requestDevice({
        filters: [{ vendorId: 0x0A5F }]
    });

    try {
        await device.open();
        if (device.configuration === null) {
            await device.selectConfiguration(1);
        }
        escolherInterface();
        await device.claimInterface(interfaceNumber);
        return device.productName;
    } catch (erro) {
        device = null;
        throw erro;
    }
}

export async function reconectarSilenciosamente() {
    const autorizados = await navigator.usb.getDevices();
    if (autorizados.length === 0) return null;

    device = autorizados[0];
    try {
        await device.open();
        if (device.configuration === null) {
            await device.selectConfiguration(1);
        }
        escolherInterface();
        await device.claimInterface(interfaceNumber);
        return device.productName;
    } catch (erro) {
        device = null;
        return null;
    }
}

export async function imprimir(zpl) {
    if (!device) {
        throw new Error('Impressora não conectada.');
    }
    const bytes = new TextEncoder().encode(zpl);
    await device.transferOut(outEndpoint, bytes);
}

export function estaConectado() {
    return device !== null;
}
