export interface ShippingAddress {
  fullName: string;
  address: string;
}

export interface OrderInfo {
  shippingAddress: ShippingAddress;
}