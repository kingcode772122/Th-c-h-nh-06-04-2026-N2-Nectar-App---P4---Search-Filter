import { Stack } from "expo-router";
import { CartProvider } from "./CartContext"; 


export default function Layout() {
  
  return (
    <CartProvider> 
  <Stack initialRouteName="homescreen" screenOptions={{ headerShown: false }}>
  <Stack.Screen name="homescreen" />  {/* 👈 THÊM DÒNG NÀY */}
  <Stack.Screen name="filter" />
  <Stack.Screen name="cart" />
  <Stack.Screen name="favourite" />
  
</Stack>
</CartProvider>
  );
}
