import React from 'react';
import { useNavigate } from 'react-router-dom';

function Unauthorized() {
  const navigate = useNavigate();

  return (
    <div style={{ 
      display: 'flex', 
      flexDirection: 'column', 
      alignItems: 'center', 
      justifyContent: 'center', 
      minHeight: '60vh',
      textAlign: 'center',
      padding: '20px'
    }}>
      <div style={{ 
        fontSize: '72px', 
        color: '#dc3545', 
        marginBottom: '20px' 
      }}>
        🚫
      </div>
      
      <h1 style={{ 
        fontSize: '36px', 
        color: '#333', 
        marginBottom: '10px' 
      }}>
        Acceso Denegado
      </h1>
      
      <p style={{ 
        fontSize: '18px', 
        color: '#666', 
        marginBottom: '30px',
        maxWidth: '600px'
      }}>
        No tienes permisos para acceder a esta página. 
        Por favor, contacta con el administrador si crees que esto es un error.
      </p>
      
      <div style={{ display: 'flex', gap: '10px' }}>
        <button 
          onClick={() => navigate(-1)}
          className="btn btn-secondary"
        >
          ← Volver Atrás
        </button>
        
        <button 
          onClick={() => navigate('/')}
          className="btn btn-primary"
        >
          🏠 Ir al Inicio
        </button>
      </div>
    </div>
  );
}

export default Unauthorized;
