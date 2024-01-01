import React from 'react';
import PopupWithForm from './PopupWithForm';
import File from './File';
import {CurrentUserContext} from '../contexts/CurrentUserContext';
import {api} from '../utils/Api';
import ReportPreviewPopup from './ReportPreviewPopup';
import ReportPopup from './ReportPopup';

export default function Main({ files, setFiles }) {
  const [isAddFilePopupOpen, setIsAddFilePopupOpen] = React.useState(false);
  const [isCreateReportPopupOpen, setIsCreateReportPopupOpen] = React.useState(false);
  const [isReportPreviewPopupOpen, setIsReportPreviewPopupOpen] = React.useState(false);
  const [file, setFile] = React.useState({});
  const [providers, setProviders] = React.useState([]);
  const [reportLink, setReportLink] = React.useState('');
  const [currentFile, setCurrentFile] = React.useState(0);
  const currentUser = React.useContext(CurrentUserContext);

  function closeAllPopups() {
    setIsAddFilePopupOpen(false);
    setIsCreateReportPopupOpen(false);
    setIsReportPreviewPopupOpen(false);
  }

  function handleAddButtonClick() {
    setIsAddFilePopupOpen(true);
  }

  function handleReportButtonClick(file) {
    api
      .getProviders(file.id)
      .then(response => {
        setProviders(response.providers);
        setCurrentFile(file);
        setIsCreateReportPopupOpen(true);
      })
      .catch(error => console.error(error));
  }

  function handleUploadFile(file) {
    setFile(file);
  }

  function sendFile(e) {
    e.preventDefault();
    const formData = new FormData();
    formData.append('upload', file, file.name);
    api
      .uploadFile(currentUser.login, formData)
      .then((response) => {
        setFiles([response.file, ...files]);
        setCurrentFile(response.file);
        closeAllPopups();
      })
      .catch(error => console.error(error));
  }

  function deleteFile(id) {
    api
      .deleteFile(id)
      .then(() => setFiles(files.filter(file => file.id !== id)))
      .catch(error => console.error(error));
  }

  function handleReportFormSubmit(fileId, format, provider) {
    const searchCallback = report => report.format === `.${format}` && report.provider === `${provider}`;
    if (currentFile.reports && currentFile.reports.some(searchCallback)) {
      const report = currentFile.reports.find(searchCallback);
      api
        .getReportFile(report.id)
        .then(response => {
          setReportLink(URL.createObjectURL(response));
          closeAllPopups();
          setIsReportPreviewPopupOpen(true);
        })
        .catch(error => console.error(error));
      return;
    }

    const request = provider === 'all' ? api.generateReport(fileId, format) :
      api.generateReportWithProvider(fileId, format, provider);

    request
      .then(response => {
        api
          .getReportFile(response.report.id)
          .then(response1 => {
            setReportLink(URL.createObjectURL(response1));
            closeAllPopups();
            setIsReportPreviewPopupOpen(true);
          })
          .catch(error => console.error(error));
      })
      .catch(error => console.error(error));
  }

  return (
    <main className="content">
      <div className="content__header">
        <h2 className="content__title">Файлы с начислениями</h2>
        <button className="content__button" onClick={handleAddButtonClick}>Загрузить файл</button>
      </div>

      <ul className="files">
        {
          files && files.map(file => (
            <File 
              key={file.id}
              file={file}
              onDelete={deleteFile}
              onReportCreate={handleReportButtonClick}
            />
          ))
        }
      </ul>

      <PopupWithForm name="" isOpen={isAddFilePopupOpen} onClose={closeAllPopups}>
        <input type="file" onChange={e => handleUploadFile(e.target.files[0])} />
        <button onClick={(e) => sendFile(e)}>Отправить</button>
      </PopupWithForm>

      <ReportPopup 
        isOpen={isCreateReportPopupOpen}
        providers={providers} 
        currentFileId={currentFile.id}
        onClose={closeAllPopups}
        onSubmit={handleReportFormSubmit}
      />

      <ReportPreviewPopup
        name=""
        reportLink={reportLink}
        isOpen={isReportPreviewPopupOpen}
        onClose={closeAllPopups}
      />
    </main>
  );
}