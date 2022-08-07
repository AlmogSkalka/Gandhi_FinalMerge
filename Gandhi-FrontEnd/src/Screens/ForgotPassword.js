import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "react-bootstrap";

const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";
// const ApiUrl = 'https://proj.ruppin.ac.il/igroup64/test2/tar6/api/'

export default function ForgotPassword() {
  const [showQuestion, setshowQuestion] = useState(false);
  const [Question, setQuestion] = useState({ questionContent: "", answer: "" });
  const [NewPasswordShow, setNewPasswordShow] = useState(false);
  const [ShowEmailInput, setShowEmailInput] = useState(true);
  const [UserEmailState, setUserEmailState] = useState("");
  const [Answer, setAnswer] = useState("");
  const navigate = useNavigate();

  const validateAnswer = (event) => {
    event.preventDefault();
    if (Question.answer === Answer) {
      alert("תשובה נכונה!");
      setShowEmailInput(false);
      setshowQuestion(false);
      setNewPasswordShow(true);
    } else {
      alert("תשובה לא נכונה!");
    }
  };
  const AnswerChange = (event) => {
    setAnswer(event.target.value);
  };
  const NewPasswordFunction = (event) => {
    event.preventDefault();
    fetch(ApiUrl + "Users?Uemail=" + UserEmailState, {
      method: "PUT",
      body: JSON.stringify(event.target.newPassword.value),
      headers: new Headers({
        "Content-Type": "application/json; charset=UTF-8",
      }),
    })
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          alert("הסיסמה השתנתה");
          navigate("/");
        },
        (error) => {
          console.log(error);
        }
      );
  };

  const submitForgotPassword = (event) => {
    event.preventDefault();
    fetch(ApiUrl + "QandA?Uemail=" + event.target.forgotPasswordEmail.value, {
      method: "GET",
      headers: new Headers({
        "Content-Type": "application/json; charset=UTF-8",
      }),
    })
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          setUserEmailState(event.target.forgotPasswordEmail.value);
          setshowQuestion(true);
          setQuestion({
            questionContent: result.Question,
            answer: result.Answer,
          });
        },
        (error) => {
          console.log(error);
        }
      );
  };

  return (
    <div>
      <Button
        className="backBtn"
        onClick={() => navigate("/")}
        style={{ position: "sticky", top: "1em", float: "right" }}
      >
        חזרה למסך התחברות
      </Button>
      <form className="form-outline mb-4 LoginForm">
        <img
          src="https://drive.google.com/uc?export=view&id=1-QhkqjYYLflhtTV4TWczQ4S3sJ6kt9K8"
          className="img-fluid"
          alt="..."
        />
      </form>

      <form
        className="form-outline mb-4 LoginForm"
        onSubmit={submitForgotPassword}
      >
        <div className="form-outline mb-4">
          {ShowEmailInput ? (
            <>
              <input
                type="email"
                name="forgotPasswordEmail"
                placeholder="כתובת אימייל"
                className="form-control HebrewInputs"
              />
              <button
                id="loginBtn"
                className="btn btn-primary btn-block RoundElements"
                type="submit"
              >
                שאל אותי שאלת זיהוי
              </button>
            </>
          ) : null}
        </div>
      </form>

      {showQuestion ? (
        <form
          className="form-outline mb-4 LoginForm"
          onSubmit={validateAnswer}
          onChange={AnswerChange}
        >
          <label> {Question.questionContent}</label>
          <br />
          <input
            className="form-control HebrewInputs"
            type="text"
            name="answerValidation"
            placeholder="תשובה לשאלת הזיהוי"
          ></input>
          <br />
          <button
            id="loginBtn"
            className="btn btn-primary btn-block RoundElements"
            type="submit"
          >
            אימות תשובה
          </button>
        </form>
      ) : null}
      {NewPasswordShow ? (
        <form
          className="form-outline mb-4 LoginForm"
          onSubmit={NewPasswordFunction}
        >
          <label> עדכון סיסמה</label>
          <br />
          <input
            className="form-control HebrewInputs"
            type="text"
            name="newPassword"
            placeholder="הכנס סיסמה חדשה פה"
          ></input>
          <br />
          <button
            id="loginBtn"
            className="btn btn-primary btn-block RoundElements"
            type="submit"
          >
            לחץ לעדכן סיסמה
          </button>
        </form>
      ) : null}
    </div>
  );
}
