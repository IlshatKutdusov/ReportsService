import React from 'react';
import PopupWithForm from './PopupWithForm';

export default function ReportPopup({isOpen, onClose, currentFileId, onSubmit, providers}) {
  const formatSelectRef = React.useRef();
  const providerSelectRef = React.useRef();

  function handleSubmit(e) {
    e.preventDefault();
    onSubmit(currentFileId, 
      formatSelectRef.current.value, 
      providerSelectRef.current.value);
  }

  return (
    <PopupWithForm 
      name="" 
      buttonText="Сгенирировать" 
      title="Сгенирировать отчёт" 
      isOpen={isOpen} 
      onClose={onClose}
      onSubmit={handleSubmit}
    >
      <p className="popup__text">Формат:</p>
      <select className="form__select" ref={formatSelectRef}>
        <option value="pdf">PDF</option>
        <option value="xlsx">Excel</option>
      </select>
      <p className="popup__text">Поставщик:</p>
      <select className="form__select" ref={providerSelectRef}>
        <option value="all">Все</option>
        {
          providers.map((provider, i) => (
            <option key={i} value={provider}>{provider}</option>
          ))
        }
      </select>
    </PopupWithForm>
  );
}