export default function ReportPreviewPopup({name, reportLink, isOpen, onClose}) {
  return (
    <div className={`popup popup_type_${name} ${isOpen && 'popup_opened'}`}>
      <div className='popup__container popup__container_type_preview'>
        <button className="popup__close-button" type="button"  aria-label="Закрыть" onClick={onClose}></button>
        <embed width="800" height="650" src={reportLink} key={reportLink}></embed>
      </div>
    </div>
  );
}