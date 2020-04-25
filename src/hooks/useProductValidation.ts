import { useState, useEffect } from 'react';

function useProductValidation(
  productNameInputValue: string,
  caloriesCost: number,
  categoryNameInputValue: string,
): [boolean, boolean, boolean] {
  const [isProductNameValid, setIsProductNameValid] = useState(false);
  const [isCaloriesCostValid, setIsCaloriesCostValid] = useState(false);
  const [isCategoryNameValid, setIsCategoryNameValid] = useState(false);

  useEffect(() => {
    const trimmedProductName = productNameInputValue.trim();
    setIsProductNameValid(trimmedProductName.length >= 4 && trimmedProductName.length <= 64);
  }, [productNameInputValue, setIsProductNameValid]);

  useEffect(() => {
    setIsCaloriesCostValid(caloriesCost >= 1 && caloriesCost <= 1000);
  }, [caloriesCost, setIsCaloriesCostValid]);

  useEffect(() => {
    setIsCategoryNameValid(categoryNameInputValue.trim() !== '');
  }, [categoryNameInputValue, setIsCategoryNameValid]);

  return [isProductNameValid, isCaloriesCostValid, isCategoryNameValid];
}

export default useProductValidation;
