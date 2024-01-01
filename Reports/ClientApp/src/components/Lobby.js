import React from 'react';
import report from '../images/report.png';

export default function Lobby(props) {
  return (
      <div className="lobby">
        <div className="welcome">
          <h1 className="welcome__title">Добро пожаловать на сервис отчётов!</h1>
          <p className="welcome__subtitle">
            Это уникальный интернет-ресурс, 
            на котором Вы можете преобразовать данные о начислениях в удобный для чтения 
            и печати отчёт в выбранном Вами формате (pdf, excel)
          </p>
          <img className="welcome__image" src={report} alt="Картинка со схемотичным изображением отчёта" />
        </div>
        <div className="form-box lobby__box">
            {props.children}
        </div>
      </div>
  );
} 