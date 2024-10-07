import { ButtonType } from './button/ButtonType';
import { ColorVariant } from './ColorVariant';

export interface MenuItem<T = void> {
  id: string;
  back?: boolean;
  context?: T;
  icon?: string;
  form?: string;
  href?: string;
  label?: string;
  routerLink?: string | (() => string);
  type?: ButtonType;
  variant?: ColorVariant;
  click?: () => void;
}
