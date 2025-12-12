import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { cortesApi, referenciasApi, materialesApi, coloresApi, tallasApi } from '../services/api';
import ImportarExcel from '../components/ImportarExcel';

function CrearCorte() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    mesa: '',
    fechaCorte: new Date().toISOString().split('T')[0],
    referenciaId: '',
    materialId: '',
    cantidadProgramada: 0,
    colores: [],
    tallas: []
  });

  const [referencias, setReferencias] = useState([]);
  const [materiales, setMateriales] = useState([]);
  const [colores, setColores] = useState([]);
  const [tallas, setTallas] = useState([]);
  const [mostrarImportarExcel, setMostrarImportarExcel] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [refResp, matResp, colResp, talResp] = await Promise.all([
        referenciasApi.getAll(),
        materialesApi.getAll(),
        coloresApi.getAll(),
        tallasApi.getAll()
      ]);
      setReferencias(refResp.data);
      setMateriales(matResp.data);
      setColores(colResp.data);
      setTallas(talResp.data);
    } catch (error) {
      console.error('Error cargando datos:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await cortesApi.create(formData);
      navigate('/cortes');
    } catch (error) {
      console.error('Error creando corte:', error);
      alert('Error al crear el corte');
    }
  };

  const handleColorChange = (colorId, cantidad) => {
    const newColores = [...formData.colores];
    const index = newColores.findIndex(c => c.colorId === parseInt(colorId));
    
    if (index >= 0) {
      newColores[index].cantidad = parseInt(cantidad) || 0;
    } else {
      newColores.push({ colorId: parseInt(colorId), cantidad: parseInt(cantidad) || 0 });
    }
    
    setFormData({ ...formData, colores: newColores.filter(c => c.cantidad > 0) });
  };

  const handleTallaChange = (tallaId, cantidad) => {
    const newTallas = [...formData.tallas];
    const index = newTallas.findIndex(t => t.tallaId === parseInt(tallaId));
    
    if (index >= 0) {
      newTallas[index].cantidad = parseInt(cantidad) || 0;
    } else {
      newTallas.push({ tallaId: parseInt(tallaId), cantidad: parseInt(cantidad) || 0 });
    }
    
    setFormData({ ...formData, tallas: newTallas.filter(t => t.cantidad > 0) });
  };

  const handleImportarExcel = (datosImportados) => {
    // Agrupar por color y talla
    const coloresMap = new Map();
    const tallasMap = new Map();
    
    datosImportados.forEach(item => {
      // Acumular colores
      if (coloresMap.has(item.colorId)) {
        coloresMap.set(item.colorId, coloresMap.get(item.colorId) + item.cantidad);
      } else {
        coloresMap.set(item.colorId, item.cantidad);
      }
      
      // Acumular tallas
      if (tallasMap.has(item.tallaId)) {
        tallasMap.set(item.tallaId, tallasMap.get(item.tallaId) + item.cantidad);
      } else {
        tallasMap.set(item.tallaId, item.cantidad);
      }
    });

    // Convertir a arrays
    const coloresImportados = Array.from(coloresMap, ([colorId, cantidad]) => ({
      colorId,
      cantidad
    }));

    const tallasImportadas = Array.from(tallasMap, ([tallaId, cantidad]) => ({
      tallaId,
      cantidad
    }));

    // Calcular cantidad total
    const cantidadTotal = datosImportados.reduce((sum, item) => sum + item.cantidad, 0);

    // Actualizar formulario
    setFormData({
      ...formData,
      colores: coloresImportados,
      tallas: tallasImportadas,
      cantidadProgramada: cantidadTotal
    });

    alert(`✅ Importados ${datosImportados.length} registros exitosamente`);
  };

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2>Crear Nuevo Corte</h2>
        <button 
          type="button"
          onClick={() => setMostrarImportarExcel(true)}
          className="btn btn-primary"
          style={{ backgroundColor: '#28a745' }}
        >
          📊 Importar desde Excel
        </button>
      </div>

      <div className="card">
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Mesa</label>
            <input
              type="text"
              value={formData.mesa}
              onChange={(e) => setFormData({ ...formData, mesa: e.target.value })}
              required
            />
          </div>

          <div className="form-group">
            <label>Fecha de Corte</label>
            <input
              type="date"
              value={formData.fechaCorte}
              onChange={(e) => setFormData({ ...formData, fechaCorte: e.target.value })}
              required
            />
          </div>

          <div className="form-group">
            <label>Referencia</label>
            <select
              value={formData.referenciaId}
              onChange={(e) => setFormData({ ...formData, referenciaId: parseInt(e.target.value) })}
              required
            >
              <option value="">Seleccione una referencia</option>
              {referencias.map(ref => (
                <option key={ref.id} value={ref.id}>{ref.nombre}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Material</label>
            <select
              value={formData.materialId}
              onChange={(e) => setFormData({ ...formData, materialId: parseInt(e.target.value) })}
              required
            >
              <option value="">Seleccione un material</option>
              {materiales.map(mat => (
                <option key={mat.id} value={mat.id}>{mat.nombre}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Cantidad Programada</label>
            <input
              type="number"
              value={formData.cantidadProgramada}
              onChange={(e) => setFormData({ ...formData, cantidadProgramada: parseInt(e.target.value) })}
              required
            />
          </div>

          <div className="form-group">
            <label>Colores y Cantidades</label>
            {colores.map(color => {
              const colorData = formData.colores.find(c => c.colorId === color.id);
              return (
                <div key={color.id} style={{ marginBottom: '10px' }}>
                  <label style={{ display: 'inline-block', width: '150px' }}>{color.nombre}</label>
                  <input
                    type="number"
                    min="0"
                    value={colorData?.cantidad || ''}
                    onChange={(e) => handleColorChange(color.id, e.target.value)}
                    style={{ width: '100px' }}
                  />
                </div>
              );
            })}
          </div>

          <div className="form-group">
            <label>Tallas y Cantidades</label>
            {tallas.map(talla => {
              const tallaData = formData.tallas.find(t => t.tallaId === talla.id);
              return (
                <div key={talla.id} style={{ marginBottom: '10px' }}>
                  <label style={{ display: 'inline-block', width: '150px' }}>{talla.nombre}</label>
                  <input
                    type="number"
                    min="0"
                    value={tallaData?.cantidad || ''}
                    onChange={(e) => handleTallaChange(talla.id, e.target.value)}
                    style={{ width: '100px' }}
                  />
                </div>
              );
            })}
          </div>

          <div style={{ display: 'flex', gap: '10px' }}>
            <button type="submit" className="btn btn-success">Guardar Corte</button>
            <button type="button" onClick={() => navigate('/cortes')} className="btn">Cancelar</button>
          </div>
        </form>
      </div>

      {mostrarImportarExcel && (
        <ImportarExcel
          onImport={handleImportarExcel}
          onClose={() => setMostrarImportarExcel(false)}
          colores={colores}
          tallas={tallas}
        />
      )}
    </div>
  );
}

export default CrearCorte;
