export function currencyFormat(symbol: string, amount: number) {
  return `${symbol}${amount.toFixed(2)}`;
}