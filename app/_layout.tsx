import { Stack } from "expo-router";

export default function Layout() {
  return (
    <Stack initialRouteName="splashscreen" screenOptions={{ headerShown: false }}>
      <Stack.Screen name="splashscreen" />
      <Stack.Screen name="onboarding" />
      <Stack.Screen name="signin" />
      <Stack.Screen name="number" />
      <Stack.Screen name="verification" />
      <Stack.Screen name="location" />
      <Stack.Screen name="login" />
      <Stack.Screen name="signup" />
    </Stack>
  );
}
