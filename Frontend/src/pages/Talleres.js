import React, { useState, useEffect } from 'react';
import { talleresApi } from '../services/api';

function Talleres() {
  const [talleres, setTalleres] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    codigo: '',
    nombre: '',
    nombreContacto: '',
    telefono: '',
    email: '',
    direccion: '',
    observaciones: ''
  });

  useEffect(() => {
    loadTalleres();
  }, []);

  const loadTalleres = async () => {
    try {
      const response = await talleresApi.getAll();
      setTalleres(response.data);
    } catch (error) {
      console.error('Error cargando talleres:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await talleresApi.create(formData);
      setShowForm(false);
      setFormData({
        codigo: '',
        nombre: '',
        nombreContacto: '',
        telefono: '',
        email: '',
        direccion: '',
        observaciones: ''
      });
      loadTalleres();
    } catch (error) {
      console.error('Error creando taller:', error);
      alert('Error al crear el taller');
    }
  };

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2>Gestión de Talleres</h2>
        <button onClick={() => setShowForm(!showForm)} className="btn btn-primary">
          {showForm ? 'Cancelar' : '+ Nuevo Taller'}
        </button>
      </div>

      {showForm && (
        <div className="card" style={{ marginBottom: '20px' }}>
          <h3>Nuevo Taller</h3>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label>Código</label>
              <input
                type="text"
                value={formData.codigo}
                onChange={(e) => setFormData({ ...formData, codigo: e.target.value })}
                required
              />
            </div>
            <div className="form-group">
              <label>Nombre</label>
              <input
                type="text"
                value={formData.nombre}
                onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
                required
              />
            </div>
            <div className="form-group">
              <label>Contacto</label>
              <input
                type="text"
                value={formData.nombreContacto}
                onChange={(e) => setFormData({ ...formData, nombreContacto: e.target.value })}
              />
            </div>
            <div className="form-group">
              <label>Teléfono</label>
              <input
                type="text"
                value={formData.telefono}
                onChange={(e) => setFormData({ ...formData, telefono: e.target.value })}
              />
            </div>
            <div className="form-group">
              <label>Email</label>
              <input
                type="email"
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
              />
            </div>
            <div className="form-group">
              <label>Dirección</label>
              <input
                type="text"
                value={formData.direccion}
                onChange={(e) => setFormData({ ...formData, direccion: e.target.value })}
              />
            </div>
            <button type="submit" className="btn btn-success">Guardar</button>
          </form>
        </div>
      )}

      <div className="card">
        <table className="table">
          <thead>
            <tr>
              <th>Código</th>
              <th>Nombre</th>
              <th>Contacto</th>
              <th>Teléfono</th>
              <th>Email</th>
            </tr>
          </thead>
          <tbody>
            {talleres.map(taller => (
              <tr key={taller.id}>
                <td>{taller.codigo}</td>
                <td>{taller.nombre}</td>
                <td>{taller.nombreContacto}</td>
                <td>{taller.telefono}</td>
                <td>{taller.email}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Talleres;
