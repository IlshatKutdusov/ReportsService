import React from "react";

export default function PopupWithForm(props) {
  const formRef = React.useRef();

  React.useEffect(() => {
    formRef.current.reset();
  }, [props.isOpen]);

  return (
    <div className={`popup popup_type_${props.name} ${props.isOpen && 'popup_opened'}`}>
    <div className="popup__container">
      <button className="popup__close-button" type="button"  aria-label="Закрыть" onClick={props.onClose}></button>
      <h2 className="popup__title">{props.title}</h2>
      <form className="popup__form" ref={formRef} name={props.name} onSubmit={props.onSubmit} action="#" noValidate>
        {props.children}
        {props.buttonText && <button className="popup__save-button" type="submit">{props.buttonText}</button>}
      </form>
    </div>
  </div>
  );
}