import React, { useState } from 'react';
import * as XLSX from 'xlsx';

function ImportarExcel({ onImport, onClose, colores, tallas }) {
  const [archivo, setArchivo] = useState(null);
  const [preview, setPreview] = useState(null);
  const [error, setError] = useState('');

  const handleArchivoChange = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    setArchivo(file);
    setError('');

    try {
      const data = await leerArchivoExcel(file);
      setPreview(data);
    } catch (err) {
      setError('Error al leer el archivo Excel: ' + err.message);
      setPreview(null);
    }
  };

  const leerArchivoExcel = (file) => {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      
      reader.onload = (e) => {
        try {
          const data = new Uint8Array(e.target.result);
          const workbook = XLSX.read(data, { type: 'array' });
          const sheetName = workbook.SheetNames[0];
          const worksheet = workbook.Sheets[sheetName];
          const jsonData = XLSX.utils.sheet_to_json(worksheet);

          // Validar estructura
          if (jsonData.length === 0) {
            reject(new Error('El archivo está vacío'));
            return;
          }

          // Validar columnas requeridas
          const columnasRequeridas = ['Lote', 'Color', 'Talla', 'Cantidad'];
          const primeraFila = jsonData[0];
          const columnasFaltantes = columnasRequeridas.filter(col => !(col in primeraFila));
          
          if (columnasFaltantes.length > 0) {
            reject(new Error(`Faltan columnas: ${columnasFaltantes.join(', ')}`));
            return;
          }

          // Procesar datos
          const datosValidos = [];
          const errores = [];

          jsonData.forEach((fila, index) => {
            const lineaNum = index + 2; // +2 porque Excel empieza en 1 y tiene header

            // Validar campos requeridos
            if (!fila.Lote || !fila.Color || !fila.Talla || !fila.Cantidad) {
              errores.push(`Línea ${lineaNum}: Faltan datos requeridos`);
              return;
            }

            // Buscar color
            const color = colores.find(c => 
              c.nombre.toLowerCase() === fila.Color.toString().toLowerCase()
            );
            if (!color) {
              errores.push(`Línea ${lineaNum}: Color "${fila.Color}" no encontrado`);
              return;
            }

            // Buscar talla
            const talla = tallas.find(t => 
              t.nombre.toLowerCase() === fila.Talla.toString().toLowerCase()
            );
            if (!talla) {
              errores.push(`Línea ${lineaNum}: Talla "${fila.Talla}" no encontrada`);
              return;
            }

            // Validar cantidad
            const cantidad = parseInt(fila.Cantidad);
            if (isNaN(cantidad) || cantidad <= 0) {
              errores.push(`Línea ${lineaNum}: Cantidad inválida`);
              return;
            }

            datosValidos.push({
              lote: fila.Lote.toString(),
              colorId: color.id,
              colorNombre: color.nombre,
              tallaId: talla.id,
              tallaNombre: talla.nombre,
              cantidad: cantidad
            });
          });

          if (errores.length > 0) {
            reject(new Error(errores.join('\n')));
            return;
          }

          resolve(datosValidos);
        } catch (err) {
          reject(err);
        }
      };

      reader.onerror = () => reject(new Error('Error al leer el archivo'));
      reader.readAsArrayBuffer(file);
    });
  };

  const handleImportar = () => {
    if (!preview || preview.length === 0) {
      setError('No hay datos para importar');
      return;
    }

    onImport(preview);
    onClose();
  };

  const descargarPlantilla = () => {
    const plantilla = [
      { Lote: 'L001', Color: 'Azul', Talla: 'M', Cantidad: 50 },
      { Lote: 'L001', Color: 'Azul', Talla: 'L', Cantidad: 30 },
      { Lote: 'L002', Color: 'Rojo', Talla: 'S', Cantidad: 40 }
    ];

    const ws = XLSX.utils.json_to_sheet(plantilla);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Plantilla');
    XLSX.writeFile(wb, 'plantilla_cortes.xlsx');
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <h3>Importar Cortes desde Excel</h3>
          <button className="close-button" onClick={onClose}>&times;</button>
        </div>

        <div className="modal-body">
          <div className="info-box">
            <p><strong>Formato requerido:</strong></p>
            <ul>
              <li>Columnas: Lote, Color, Talla, Cantidad</li>
              <li>Los colores y tallas deben existir en el sistema</li>
              <li>Las cantidades deben ser números positivos</li>
            </ul>
            <button onClick={descargarPlantilla} className="btn-secondary">
              📥 Descargar Plantilla Excel
            </button>
          </div>

          <div className="form-group">
            <label>Seleccionar archivo Excel:</label>
            <input
              type="file"
              accept=".xlsx, .xls"
              onChange={handleArchivoChange}
              className="form-control"
            />
          </div>

          {error && (
            <div className="alert alert-error" style={{ whiteSpace: 'pre-line' }}>
              {error}
            </div>
          )}

          {preview && preview.length > 0 && (
            <div className="preview-section">
              <h4>Vista previa ({preview.length} registros):</h4>
              <div style={{ maxHeight: '300px', overflowY: 'auto' }}>
                <table className="table">
                  <thead>
                    <tr>
                      <th>Lote</th>
                      <th>Color</th>
                      <th>Talla</th>
                      <th>Cantidad</th>
                    </tr>
                  </thead>
                  <tbody>
                    {preview.map((item, index) => (
                      <tr key={index}>
                        <td>{item.lote}</td>
                        <td>{item.colorNombre}</td>
                        <td>{item.tallaNombre}</td>
                        <td>{item.cantidad}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}
        </div>

        <div className="modal-footer">
          <button onClick={onClose} className="btn-secondary">
            Cancelar
          </button>
          <button 
            onClick={handleImportar} 
            className="btn-primary"
            disabled={!preview || preview.length === 0}
          >
            Importar {preview?.length || 0} registros
          </button>
        </div>
      </div>
    </div>
  );
}

export default ImportarExcel;
