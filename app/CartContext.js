import { createContext, useState } from 'react';

export const CartContext = createContext();

export const CartProvider = ({ children }) => {
  const [cart, setCart] = useState([]);

  const addToCart = (product) => {
    setCart(prev => {
      const exist = prev.find(item => item.id === product.id);
      if (exist) {
        return prev.map(item =>
          item.id === product.id
            ? { ...item, qty: (item.qty ?? 1) + 1 }
            : item
        );
      }
      return [...prev, { ...product, qty: 1 }];
    });
  };

  const increaseQty = (id) => {
    setCart(prev =>
      prev.map(item =>
        item.id === id ? { ...item, qty: (item.qty ?? 1) + 1 } : item
      )
    );
  };

  const decreaseQty = (id) => {
    setCart(prev =>
      prev
        .map(item =>
          item.id === id ? { ...item, qty: (item.qty ?? 1) - 1 } : item
        )
        .filter(item => item.qty > 0)
    );
  };

  const removeItem = (id) => {
    setCart(prev => prev.filter(item => item.id !== id));
  };

  return (
    <CartContext.Provider value={{ cart, addToCart, increaseQty, decreaseQty, removeItem }}>
      {children}
    </CartContext.Provider>
  );
};