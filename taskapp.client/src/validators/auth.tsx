
interface ValidationErrors {
  name?: string;
  email?: string;
  password?: string;
  confirmPassword?: string;
}

export function validateRegister(formData: {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
}): ValidationErrors {
  const errors: ValidationErrors = {};

  if (!formData.name.trim()) errors.name = "Name is required.";
  else if (formData.name.length > 100) errors.name = "Name cannot exceed 100 characters.";

  if (!formData.email.trim()) errors.email = "Email is required.";
  else if (!/^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/.test(formData.email)) errors.email = "Invalid email format.";

  if (!formData.password) errors.password = "Password is required.";
  else if (formData.password.length < 8) errors.password = "Password must be at least 8 characters long.";

  if (formData.password !== formData.confirmPassword) errors.confirmPassword = "Passwords do not match.";

  return errors;
}
