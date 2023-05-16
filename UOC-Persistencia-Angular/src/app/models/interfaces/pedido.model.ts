export interface Pedido {
    id?: number,
    displayName: string,
    dateOrder: Date,
    company: string,
    partner: string,
    amountUntaxerd: number,
    amountTotal: number,
    taxGroupName: string,
    deliveryStatus: string,
    state?: string
}
