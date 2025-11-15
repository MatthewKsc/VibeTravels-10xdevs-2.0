import { NavigationOption } from "./navigation.model";

export const unauthenticatedOptions: NavigationOption[] = [
  { label: 'Sign In', route: '/auth/sign-in', icon: 'login' },
];

export const authenticatedOptions: NavigationOption[] = [
  { label: 'Notes', route: '/notes', icon: 'description' },
  { label: 'Plans', route: '/plans', icon: 'map' },
];