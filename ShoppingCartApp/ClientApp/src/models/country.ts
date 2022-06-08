export interface Country {
  id: number;
  name: string;
  currencyRate: number;
  currencySymbol: string;
  currencyCode: string;
}

export function calculatePrice(price: number, currencyRate: number): number {
  return price * currencyRate;
}