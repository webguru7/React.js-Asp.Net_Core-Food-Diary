import { useState, useEffect } from 'react';
import { BindingCreatorFunction, InputHook, InputOptions } from './types';

export default function createInputHook<TValue, TBindingProps>(
  createBinding: BindingCreatorFunction<TValue, TBindingProps>,
): InputHook<TValue, TBindingProps, InputOptions<TValue>> {
  return (initialValue, options = {}) => {
    const [value, setValue] = useState(initialValue);
    const [isInitialized, setIsInitialized] = useState(false);
    const { afterChange } = options;

    useEffect(() => {
      setIsInitialized(true);
    }, []);

    useEffect(() => {
      if (isInitialized && afterChange) {
        afterChange(value);
      }
    }, [value]);

    return [value, setValue, () => createBinding(value, setValue)];
  };
}