export function currencyFormat(symbol: string, amount: number) {
  return `${symbol}${amount.toFixed(2)}`;
}

// check if code is run by jest
export function isJest() {
  const jestDefined = typeof jest !== 'undefined';
  return jestDefined;
}
