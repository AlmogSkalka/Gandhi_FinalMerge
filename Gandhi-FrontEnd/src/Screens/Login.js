import React, { useState } from 'react'
import { authentication, db } from '../Comps/External Connections/FireBase-config'
import {
  signInWithPopup,
  GoogleAuthProvider,
  FacebookAuthProvider,
  signInWithEmailAndPassword
}
  from "firebase/auth";
import { Link, useNavigate } from 'react-router-dom';
import { doc, updateDoc } from 'firebase/firestore'
import { Col, Row, Spinner } from 'react-bootstrap';
import * as BiIcons from "react-icons/bi";

// const ApiUrl = "https://localhost:44315/api/";
const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";

export default function Login() {
  const [ShowPassword, setShowPassword] = useState(false)
  const [UserObject, setUserObject] = useState([])

  const navigate = useNavigate();

  const [data, setData] = useState({
    email: "",
    password: "",
    error: null,
    loading: false
  })

  const { email, password, error, loading } = data;

  const handleChange = e => {
    setData({ ...data, [e.target.name]: e.target.value })
  }

  const signWithExternals = (externalObject) => {
    fetch(ApiUrl + "Users?email=" + externalObject.email, {
      method: 'GET',
      headers: new Headers({
        'Content-Type': 'application/json; charset=UTF-8'
      })
    })
      .then(res => {
        return res.json()
      })
      .then(
        (result) => {

          if (result.UserId === 0) {
            navigate("/Registration", { state: externalObject });
          }
          else if (result.UserId !== 0) {
            FireBaseLogin();
            navigate('/MainDashboard');
          }
        },
        (error) => {
          console.log(error)
        });
  }

  const signInWithGoogle = () => {
    const provider = new GoogleAuthProvider();
    signInWithPopup(authentication, provider)
      .then((re) => {
        let external = {
          email: re.user.email, FullName: re.user.displayName
        }
        signWithExternals(external)
      })
      .catch((err) => {
        console.log(err.message)
      })
  }

  const signInWithFaceBook = () => {
    const provider = new FacebookAuthProvider();
    signInWithPopup(authentication, provider)
      .then((re) => {
        let external = {
          email: re.user.email, FullName: re.user.displayName
        }
        signWithExternals(external)
      })
      .catch((err) => {
        console.log(err)
      })

  }

  const userLogsIn = async (event) => {
    event.preventDefault();
    setData({ ...data, error: null, loading: true })
    if (!email || !password) {
      setData({ ...data, error: "חובה למלא את כל השורות!" })
      alert({ error })
    }
    else {
      const User = {
        email: event.target.email.value,
        pswrd: event.target.password.value
      }
      fetch(ApiUrl + "Users?email=" + User.email + "&password=" + User.pswrd, {
        method: 'GET',
        headers: new Headers({
          'Content-Type': 'application/json; charset=UTF-8'
        })
      })
        .then(res => {
          return res.json()
        })
        .then(
          (result) => {
            if (result.UserId > 0) {
              localStorage.setItem("user", JSON.stringify(result))
              setUserObject(result)
              FireBaseLogin();
            }
            else if (result.UserId === 0) {
              alert("הסיסמא או שם המשתמש אינם נכונים")
              window.location.reload()
            }
            else if (!result.UserId) {
              alert("הסיסמא או שם המשתמש אינם נכונים")
              window.location.reload()
            }
          },
          (error) => {
            console.log(error)

          });

    }
  }

  const FireBaseLogin = async () => {
    const User = {
      email: UserObject.Email,
      pswrd: UserObject.Password
    }
    try {
      const result = await signInWithEmailAndPassword(
        authentication,
        User.email,
        User.pswrd
      );
      await updateDoc(doc(db, 'users', result.user.uid), {
        isOnline: true,
        GandhiUserId: UserObject.UserId,
        GandhiProfilePic: UserObject.ProfilePicUrl
      });
      setData({
        email: "",
        password: "",
        error: null,
        loading: false
      });
      navigate('/MainDashboard');
    }
    catch (err) { console.log("tryed to log in firebase user and there's an err:", err) }
  }

  if (localStorage.getItem("user") === null) {
    return (
      <>
        <form onSubmit={userLogsIn} style={{ height: '100%' }} className='LoginForm'>
          <img alt='Gandhi LOGO' src="https://drive.google.com/uc?export=view&id=1-QhkqjYYLflhtTV4TWczQ4S3sJ6kt9K8" className="img-fluid" />
          <div className="form-outline mb-4">
            <input type="email" name='email' placeholder='כתובת אימייל' className="form-control HebrewInputs" onChange={handleChange} />
          </div>

          <div className="form-outline mb-4">
            <Row>
              <Col className='col col-sm-2'>
                {
                  ShowPassword ?
                    <BiIcons.BiShow onClick={() => setShowPassword(!ShowPassword)} />
                    :
                    <BiIcons.BiShowAlt onClick={() => setShowPassword(!ShowPassword)} />
                }
              </Col>
              <Col className='col col-sm-10'>
                <input
                  type={ShowPassword ? 'text' : 'password'}
                  name='password'
                  placeholder="סיסמה"
                  autoComplete="on" onChange={handleChange}
                  className="form-control HebrewInputs" />
              </Col>
            </Row>



          </div>
          <button type="submit" id="loginBtn" className="btn btn-primary btn-block RoundElements" disabled={loading}>
            {
              loading ?
                <Spinner animation="border" />
                :
                "התחברות"
            }
          </button>
          <br />
          <br />
          <div className="col">
            <Link className='loginHref' to="ForgotPassword">
              שכחתי סיסמה
            </Link>
          </div>
          <br />
          <div className="col">
            עדיין לא נרשמת? {"  "}
            <Link className='loginHref' to="Registration">
              הרשמה לאפליקציה
            </Link>
          </div>
          <br />
          <br />
          <div>
            הרשמה באמצעות
            <br /><br />
            <img className="LoginElements" alt="Facebook BTN" onClick={signInWithFaceBook} src="https://img.icons8.com/color/48/000000/facebook-new.png" />{" "}{" "}
            <img className="LoginElements" alt="Google BTN" onClick={signInWithGoogle} src="https://img.icons8.com/color/48/000000/google-logo.png" />
          </div>
        </form>

      </>
    )
  }

  if (localStorage.getItem("user") !== null) {
    FireBaseLogin();
  }

}
