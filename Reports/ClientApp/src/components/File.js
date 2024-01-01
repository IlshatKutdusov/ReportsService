import moment from 'moment';
import React from 'react';
import { api } from '../utils/Api';

export default function File({file: fileData, onDelete, onReportCreate}) {
  const [link, setLink] = React.useState('#');

  function handleDelete() {
    onDelete(fileData.id);
  }

  function handleReportCreate() {
    onReportCreate(fileData);
  }

  function formatDate(dateString) {
    return moment(dateString).format('DD.MM.YYYY HH:mm');
  }

  React.useEffect(() => {
    api
      .getFile(fileData.id)
      .then(response => {
        setLink(URL.createObjectURL(response));
      })
      .catch(error => console.error(error));
  }, []);
  
  return (
    <div className="file">
      <div className="file__text">
        <a className="file__link" href={link} download={fileData.name}><h3 className="file__name">{fileData.name.split('_')[1]}</h3></a>
        <p className="file__info">{`${formatDate(fileData.dateCreated)}, ${fileData.size} MB`}</p>
      </div>   
      <div className="file__button-container">
        <button className="file__report-button" title="Сгенерировать отчёт" onClick={handleReportCreate}></button>
        <button className="file__remove-button" title="Удалить" onClick={handleDelete}></button>
      </div>
    </div>
  );
}