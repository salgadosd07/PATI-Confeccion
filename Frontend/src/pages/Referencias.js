import React, { useState, useEffect } from 'react';
import { referenciasApi } from '../services/api';

function Referencias() {
  const [referencias, setReferencias] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    codigo: '',
    nombre: '',
    descripcion: '',
    tipoPrenda: ''
  });

  useEffect(() => {
    loadReferencias();
  }, []);

  const loadReferencias = async () => {
    try {
      const response = await referenciasApi.getAll();
      setReferencias(response.data);
    } catch (error) {
      console.error('Error cargando referencias:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await referenciasApi.create(formData);
      setShowForm(false);
      setFormData({
        codigo: '',
        nombre: '',
        descripcion: '',
        tipoPrenda: ''
      });
      loadReferencias();
    } catch (error) {
      console.error('Error creando referencia:', error);
      alert('Error al crear la referencia');
    }
  };

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2>Gestión de Referencias</h2>
        <button onClick={() => setShowForm(!showForm)} className="btn btn-primary">
          {showForm ? 'Cancelar' : '+ Nueva Referencia'}
        </button>
      </div>

      {showForm && (
        <div className="card" style={{ marginBottom: '20px' }}>
          <h3>Nueva Referencia</h3>
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
              <label>Tipo de Prenda</label>
              <input
                type="text"
                value={formData.tipoPrenda}
                onChange={(e) => setFormData({ ...formData, tipoPrenda: e.target.value })}
                required
              />
            </div>
            <div className="form-group">
              <label>Descripción</label>
              <textarea
                value={formData.descripcion}
                onChange={(e) => setFormData({ ...formData, descripcion: e.target.value })}
                rows="3"
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
              <th>Tipo de Prenda</th>
              <th>Descripción</th>
            </tr>
          </thead>
          <tbody>
            {referencias.map(ref => (
              <tr key={ref.id}>
                <td>{ref.codigo}</td>
                <td>{ref.nombre}</td>
                <td>{ref.tipoPrenda}</td>
                <td>{ref.descripcion}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Referencias;
