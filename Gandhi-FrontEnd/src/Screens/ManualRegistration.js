import React, { useState, useEffect, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { useLocation } from "react-router";
import * as BiIcons from "react-icons/bi";
import Modal from "react-modal";
import { Row, Col, Button, Spinner } from "react-bootstrap";
import Form from "react-bootstrap/Form";
import { createUserWithEmailAndPassword } from "firebase/auth";
import { setDoc, doc, Timestamp } from "firebase/firestore";
import {
  authentication,
  db,
} from "../Comps/External Connections/FireBase-config";
import Cropper from "react-easy-crop";
import getCroppedImg from "../cropImage";
import Slider from "@mui/material/Slider";

const axios = require("axios").default;
const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";
const ImagesUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar1/api/image";
// const ApiUrl = 'https://proj.ruppin.ac.il/igroup64/test2/tar6/api/'
export default function ManualRegistration() {
  const [croppedAreaPixels, setcroppedAreaPixels] = useState({});
  const [showModal, setshowModal] = useState(false);
  const [Latitude, setLatitude] = useState("");
  const [Longitude, setLongitude] = useState("");
  const [ProfilePic, setProfilePic] = useState("");
  const [ExternalFullName, setFullName] = useState("");
  const [ExternalEmail, setEmail] = useState("");
  const [UserId, setUserId] = useState("");
  const [ValidationQuestions, setValidationQuestions] = useState([]);
  const [ShowPassword, setShowPassword] = useState(false);
  const [crop, setCrop] = useState({ x: 0, y: 0 });
  const [zoom, setZoom] = useState(1);
  const [CroppedImages, setCroppedImages] = useState([]);
  const [CroppingImg, setCroppingImg] = useState(null);
  const { state } = useLocation();
  const navigate = useNavigate();

  const [data, setData] = useState({
    name: "",
    email: "",
    password: "",
    error: null,
    loading: false,
  });
  const { loading } = data;
  //eslint-disable-next-line react-hooks/exhaustive-deps
  const onCropComplete = useCallback((croppedArea, croppedAreaPixels) => {
    setcroppedAreaPixels(croppedAreaPixels);
  });
  const handleChange = (e) => {
    setData({ ...data, [e.target.name]: e.target.value });
  };
  const handleOpenModal = () => {
    setshowModal(true);
  };
  const handleCloseModal = () => {
    setshowModal(false);
  };
  const showCroppedImage = async () => {
    try {
      const croppedImage = await getCroppedImg(
        CroppingImg,
        croppedAreaPixels,
        0
      );
      let tmpCroppedImgsArr = [];
      CroppedImages.forEach((element) => {
        tmpCroppedImgsArr.push(element);
      });
      tmpCroppedImgsArr.push(croppedImage);
      setCroppedImages(tmpCroppedImgsArr);
      setshowModal(false);
    } catch (e) {
      console.error(e);
    }
  };
  const readFile = (file) => {
    return new Promise((resolve) => {
      const reader = new FileReader();
      reader.addEventListener("load", () => resolve(reader.result), false);
      reader.readAsDataURL(file);
    });
  };
  const onFileChange = async (event) => {
    let tmpEvent = event.target.files;
    for (let i = 0; i < tmpEvent.length; i++) {
      const file = tmpEvent[i];
      let imageDataUrl = await readFile(file);
      setCroppingImg(imageDataUrl);
      setCrop({ x: 0, y: 0 });
      setZoom(1);
      handleOpenModal();
    }
  };

  useEffect(() => {
    if (ProfilePic !== "" && UserId > 0) {
      const formData = new FormData();
      try {
        CroppedImages.forEach((element) => {
          formData.append(
            "myFile",
            element,
            "UsersProfilePics " + UserId + " .jpeg"
          );
        });
      } catch (error) {
        console.log(
          "Error occured when tried to upload user cropped photo: ",
          error
        );
      }

      axios
        .post(ImagesUrl, formData)
        .then(function (response) {
          fetch(ApiUrl + "Users?UserId=" + UserId, {
            method: "POST",
            body: JSON.stringify(response.data),
            headers: new Headers({
              "Content-Type": "application/json; charset=UTF-8",
              Accept: "application/json; charset=UTF-8",
            }),
          })
            .then((res) => {
              return res.json();
            })
            .then(
              (result) => {
                navigate("/");
              },
              (error) => { }
            );
        })
        .catch(function (error) {
          console.log(error);
        });
    }
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, [ProfilePic]);

  useEffect(() => {
    navigator.geolocation.getCurrentPosition(function (position) {
      setLatitude(position.coords.latitude);
      setLongitude(position.coords.longitude);
    });
    GetValidationQuestions();
    if (state !== null || state !== undefined) {
      setEmail(state.email);
      setFullName(state.FullName);
    }
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const GetValidationQuestions = () => {
    fetch(ApiUrl + "Users/", {
      method: "GET",
      headers: new Headers({
        "Content-Type": "application/json; charset=UTF-8",
        Accept: "application/json; charset=UTF-8",
      }),
    })
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          let tmpQuestionsArr = [];
          for (let i = 0; i < result.length; i++) {
            tmpQuestionsArr.push(result[i]);
          }
          setValidationQuestions(tmpQuestionsArr);
        },
        (error) => { }
      );
  };

  const manRegSub = async (event) => {
    event.preventDefault();
    setData({ ...data, error: null, loading: true });
    // if (!name || !email || !password) {
    //     setData({ ...data, error: "חובה למלא את כל השורות!" })
    //     alert("Not all fields inserted for some reason: ", { error })
    // }
    // else {
    navigator.geolocation.getCurrentPosition(function (position) {
      setLatitude(position.coords.latitude);
      setLongitude(position.coords.longitude);
    });
    const manualNewUserRegistrationDetails = {
      Address:
        event.target.newUserStreet.value +
        " " +
        event.target.newUserStreetNum.value,
      City: event.target.newUserCity.value,
      CoinsCredit: 0,
      DateOfBirth: event.target.newUserBirthdate.value,
      Email: event.target.email.value,
      FullName: event.target.name.value,
      Gender: event.target.gender.value,
      LocationX: Latitude,
      LocationY: Longitude,
      Password: event.target.password.value,
      PersonalStatus: event.target.status.value,
      ProfilePicUrl: "",
      ValidationQuestion: event.target.newUserValidationQuestion.value,
      ValidationAnswer: event.target.newUserValidationAnswer.value,
    };

    const email = event.target.email.value;
    const Password = event.target.password.value;
    const name = event.target.name.value;
    try {
      const result = await createUserWithEmailAndPassword(
        authentication,
        email,
        Password
      );
      await setDoc(doc(db, "users", result.user.uid), {
        uid: result.user.uid,
        name,
        email,
        createdAt: Timestamp.fromDate(new Date()),
        isOnline: true,
        GandhiUserId: 0,
      });
      setData({
        name: "",
        email: "",
        password: "",
        error: null,
        loading: false,
      });
      PostNewUser(manualNewUserRegistrationDetails);
    } catch (err) {
      setData({ ...data, error: err.message, loading: false });
      console.log(
        "tryed to create firebase user and there's an err:",
        err.message
      );
    }
    // }
  };

  const PostNewUser = (NewUser) => {
    fetch(ApiUrl + "Users/", {
      method: "POST",
      body: JSON.stringify(NewUser),
      headers: new Headers({
        "Content-Type": "application/json; charset=UTF-8",
      }),
    })
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          if (result !== 0) {
            alert("נרשמת בהצלחה!");
            setUserId(result);
            setProfilePic(CroppedImages);
            navigate("/");
          } else if (result === 0) {
            alert("קיים אימייל זהה במסד הנתונים");
          }
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
      <form
        className="form-outline mb-4 LoginForm"
        onSubmit={manRegSub}
        style={{ backgroundColor: "white" }}
      >
        {/* <Link id='back2LoginBTN' to="/">
                    חזרה לדף התחברות
                </Link> */}
        <img
          src="https://drive.google.com/uc?export=view&id=1-QhkqjYYLflhtTV4TWczQ4S3sJ6kt9K8"
          style={{
            borderRadius: "15px",
            margin: "0px auto",
            width: "45%",
            textAlign: "center",
          }}
          className="img-fluid"
          alt="..."
        />
        {/* If using Google/FB Credentials */}
        {ExternalEmail !== "" && ExternalFullName !== "" ? (
          <>
            <div className="form-outline mb-4">
              <input
                type="email"
                className="form-control HebrewInputs"
                value={ExternalEmail}
                disabled={true}
                name="email"
              />
            </div>
            <Row>
              <Col>
                {/* Full Name */}
                <div className="form-outline mb-4">
                  <input
                    type="text"
                    className="form-control HebrewInputs"
                    value={ExternalFullName}
                    disabled={true}
                    name="name"
                  />
                </div>
              </Col>
            </Row>
          </>
        ) : (
          <>
            {/* Email */}
            <div className="form-outline mb-4">
              <input
                type="email"
                className="form-control HebrewInputs"
                name="email"
                required
                placeholder="כתובת דואר אלקטרוני"
                onChange={handleChange}
              />
            </div>
            <Row>
              <Col>
                {/* Full Name */}
                <div className="form-outline mb-4">
                  <input
                    type="text"
                    className="form-control HebrewInputs"
                    name="name"
                    required
                    placeholder="שם מלא"
                    onChange={handleChange}
                  />
                </div>
              </Col>
            </Row>
          </>
        )}
        <Row>
          <Col>
            {/* Profile Pic */}
            <div className="form-outline mb-4">
              <p>צירוף תמונת פרופיל</p>
              <input
                required
                className="form-control HebrewInputs"
                type="file"
                onChange={onFileChange}
                accept={"image/*"}
              />
            </div>
          </Col>
          <Col>
            {/* Birthday */}
            <div className="form-outline mb-4">
              <p>תאריך לידה</p>
              <input
                type="date"
                className="form-control HebrewInputs"
                name="newUserBirthdate"
                required
              />
            </div>
          </Col>
        </Row>
        <Row>
          <Col>
            {/* City */}
            <div className="form-outline mb-4">
              <input
                type="text"
                className="form-control HebrewInputs"
                name="newUserCity"
                required
                placeholder="עיר מגורים"
              />
            </div>
          </Col>
        </Row>
        <Row>
          <Col>
            {/* Street Number */}
            <div className="form-outline mb-4">
              <input
                type="number"
                className="form-control HebrewInputs"
                name="newUserStreetNum"
                required
                placeholder="מספר בית"
                onWheel={(e) => e.target.blur()}
              />
            </div>
          </Col>
          <Col>
            {/* Street */}
            <div className="form-outline mb-4">
              <input
                type="text"
                className="form-control HebrewInputs"
                name="newUserStreet"
                required
                placeholder="רחוב"
              />
            </div>
          </Col>
        </Row>

        <Row>
          <Col>
            {/* Gender */}
            <div className="form-outline mb-4">
              <div id="select-wrapper-781747" className="select-wrapper">
                <div className="form-outline">
                  <span className="select-arrow"></span>
                  <div className="form-notch">
                    <div
                      className="form-notch-leading"
                      style={{ width: "9px" }}
                    ></div>
                    <div
                      className="form-notch-middle"
                      style={{ width: "0px" }}
                    ></div>
                    <div className="form-notch-trailing"></div>
                  </div>
                  <div className="form-label select-fake-value">מגדר</div>
                </div>
                <Form.Select
                  required
                  name="gender"
                  className="select select-initialized"
                >
                  <option key="m" value="m">
                    זכר
                  </option>
                  <option key="f" value="f">
                    נקבה
                  </option>
                </Form.Select>
              </div>
            </div>
          </Col>
          <Col>
            {/* Kids/NoKids */}
            <div className="form-outline mb-4">
              <div id="select-wrapper-781747" className="select-wrapper">
                <div className="form-outline">
                  <span className="select-arrow"></span>
                  <div className="form-notch">
                    <div
                      className="form-notch-leading"
                      style={{ width: "9px" }}
                    ></div>
                    <div
                      className="form-notch-middle"
                      style={{ width: "0px" }}
                    ></div>
                    <div className="form-notch-trailing"></div>
                  </div>
                  <div className="form-label select-fake-value">מצב משפחתי</div>
                </div>
                <Form.Select
                  name="status"
                  required
                  className="select select-initialized"
                >
                  <option value="0"> אין ילדים קטנים</option>
                  <option value="1"> יש ילדים קטנים</option>
                </Form.Select>
              </div>
            </div>
          </Col>
        </Row>
        {/* Password */}
        <div className="form-outline mb-4">
          <p>סיסמה</p>
          <Row>
            {/* ShowPassword */}
            <Col>
              {ShowPassword ? (
                <BiIcons.BiShow
                  onClick={() => setShowPassword(!ShowPassword)}
                />
              ) : (
                <BiIcons.BiShowAlt
                  onClick={() => setShowPassword(!ShowPassword)}
                />
              )}
            </Col>
            {/* Password Field */}
            <Col className="col">
              <input
                type={ShowPassword ? "text" : "password"}
                className="form-control HebrewInputs"
                name="password"
                placeholder="סיסמה"
                autoComplete="on"
                onChange={handleChange}
                required
              />
            </Col>
          </Row>
        </div>
        <Row>
          <Col>
            {/* Validation Answer */}
            <div className="form-outline mb-4">
              <p>תשובה לשאלת אימות</p>
              <input
                type="text"
                required
                className="form-control HebrewInputs"
                name="newUserValidationAnswer"
              />
            </div>
          </Col>
          <Col>
            {/* Validation Q & A */}
            <div className="form-outline mb-4">
              <div id="select-wrapper-781747" className="select-wrapper">
                <div className="form-outline">
                  <span className="select-arrow"></span>
                  <div className="form-notch">
                    <div
                      className="form-notch-leading"
                      style={{ width: "9px" }}
                    ></div>
                    <div
                      className="form-notch-middle"
                      style={{ width: "0px" }}
                    ></div>
                    <div className="form-notch-trailing"></div>
                  </div>
                  <div className="form-label select-fake-value">שאלת זיהוי</div>
                </div>
                <Form.Select
                  required
                  name="newUserValidationQuestion"
                  className="select select-initialized"
                >
                  {ValidationQuestions.length > 1 ? (
                    ValidationQuestions.map((question, ind) => (
                      <option key={ind} value={question.Qid1}>
                        {" "}
                        {question.Question}
                      </option>
                    ))
                  ) : (
                    <option key="null" value="null">
                      {" "}
                      השאלות זיהוי אמורות להופיע פה
                    </option>
                  )}
                </Form.Select>
              </div>
            </div>
          </Col>
        </Row>
        <button
          disabled={loading}
          type="submit"
          id="loginBtn"
          className="btn btn-primary btn-block RoundElements"
        >
          {loading ? <Spinner animation="border" /> : "הרשמה"}
        </button>
      </form>
      <Modal
        isOpen={showModal}
        contentLabel="Minimal Modal Example"
        style={{ height: "35em", width: "35em", paddingTop: "50em" }}
      >
        <Button
          variant="primary"
          id="CropImageModalBTN"
          onClick={handleCloseModal}
        >
          זו לא התמונה הנכונה
        </Button>
        {CroppingImg ? (
          <>
            <div className="crop-container">
              <Cropper
                image={CroppingImg}
                crop={crop}
                zoom={zoom}
                aspect={3 / 4}
                onCropChange={setCrop}
                onCropComplete={onCropComplete}
                onZoomChange={setZoom}
                initialCroppedAreaPercentages={undefined}
              />
            </div>
            <div className="controls">
              <Slider
                value={zoom}
                min={1}
                max={3}
                step={0.1}
                aria-labelledby="Zoom"
                onChange={(e, zoom) => setZoom(zoom)}
                classes={{ container: "slider" }}
              />
            </div>
            <Button
              onClick={showCroppedImage}
              color="primary"
              id="CropImageModalBTN"
            >
              גזירת תמונה
            </Button>
          </>
        ) : null}
      </Modal>
    </div>
  );
}
