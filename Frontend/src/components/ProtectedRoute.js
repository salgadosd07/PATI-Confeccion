import React from 'react';
import { Navigate } from 'react-router-dom';

function ProtectedRoute({ children, roles }) {
  const token = localStorage.getItem('token');
  const userStr = localStorage.getItem('user');
  
  // Si no hay token, redirigir al login
  if (!token) {
    return <Navigate to="/login" replace />;
  }

  // Si se especifican roles, verificar que el usuario tenga uno de ellos
  if (roles && roles.length > 0) {
    try {
      const user = userStr ? JSON.parse(userStr) : null;
      
      if (!user || !user.role) {
        return <Navigate to="/unauthorized" replace />;
      }

      // Verificar si el rol del usuario está en la lista de roles permitidos
      const hasPermission = roles.includes(user.role);
      
      if (!hasPermission) {
        return <Navigate to="/unauthorized" replace />;
      }
    } catch (error) {
      console.error('Error al verificar permisos:', error);
      return <Navigate to="/login" replace />;
    }
  }

  return children;
}

export default ProtectedRoute;
