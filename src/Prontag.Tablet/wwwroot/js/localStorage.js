export function definir(chave, valor) {
    localStorage.setItem(chave, valor);
}
export function obter(chave) {
    return localStorage.getItem(chave);
}
export function remover(chave) {
    localStorage.removeItem(chave);
}
