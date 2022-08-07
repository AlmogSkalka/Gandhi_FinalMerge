import React from "react";
import * as FaIcons from "react-icons/fa";
import * as FiIcons from "react-icons/fi";
import * as GiIcons from "react-icons/gi";
import * as BsIcons from "react-icons/bs";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import "./AppNavbar.css";
const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";

export default function AppNavbar() {
  const UserLocalStorage = JSON.parse(localStorage.getItem("user"));
  const navigate = useNavigate();

  const CategoriesSearchPage = (depId) => {
    fetch(ApiUrl + "Departments?departmentId=" + depId, {
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
          navigate("/Categories", { state: result });
        },
        (error) => {
          console.log(error);
        }
      );
  };

  if (UserLocalStorage !== undefined && UserLocalStorage !== null) {
    return (
      <div className="navbar-wrapper">
        <div className="navbar-content">
          <div className="navbar-logo">
            <Link to='/MainDashboard'>
              <img
                src="assets/img/gallery/gandhi logo.png"
                alt="logo"
                height="80"
              />
            </Link>
          </div>
          <div className="navbar-menu">
            <button
              className="navbar-menu-button"
              onClick={() => CategoriesSearchPage(1)}
            >
              נשים
            </button>
            <button
              className="navbar-menu-button"
              onClick={() => CategoriesSearchPage(2)}
            >
              גברים
            </button>
            <button
              className="navbar-menu-button"
              onClick={() => CategoriesSearchPage(3)}
            >
              ילדים
            </button>
          </div>
          <div className="navbar-icons">
            <Link to="/Disconnect">
              <FiIcons.FiLogOut className="feather feather-phone me-3"></FiIcons.FiLogOut>
            </Link>

            <Link to="/NewItemPage">
              <GiIcons.GiMailShirt className="feather feather-phone me-3"></GiIcons.GiMailShirt>
            </Link>

            <Link to="/Chat">
              <BsIcons.BsChatLeftText className="feather feather-phone me-3"></BsIcons.BsChatLeftText>
            </Link>

            <Link to="/Search">
              <FaIcons.FaSearch className="feather feather-phone me-3"></FaIcons.FaSearch>
            </Link>
          </div>
          <div className="mobile-menu">
            <div className="mobile-menu-menu">
              <button
                className="navbar-menu-button"
                onClick={() => CategoriesSearchPage(1)}
              >
                נשים
              </button>
              <button
                className="navbar-menu-button"
                onClick={() => CategoriesSearchPage(2)}
              >
                גברים
              </button>
              <button
                className="navbar-menu-button"
                onClick={() => CategoriesSearchPage(3)}
              >
                ילדים
              </button>
            </div>
            <div className="mobile-menu-icons">
              <Link to="/Disconnect">
                <FiIcons.FiLogOut className="feather feather-phone me-3"></FiIcons.FiLogOut>
              </Link>

              <Link to="/NewItemPage">
                <GiIcons.GiMailShirt className="feather feather-phone me-3"></GiIcons.GiMailShirt>
              </Link>

              <Link to="/Chat">
                <BsIcons.BsChatLeftText className="feather feather-phone me-3"></BsIcons.BsChatLeftText>
              </Link>

              <Link to="/Search">
                <FaIcons.FaSearch className="feather feather-phone me-3"></FaIcons.FaSearch>
              </Link>
            </div>
          </div>
          <div className="navbar-user">
            <Link to="/PersonalArea">
              {UserLocalStorage?.ProfilePicUrl ? (
                <img
                  className="user-avatar"
                  src={UserLocalStorage.ProfilePicUrl}
                  alt="user"
                />
              ) : (
                <FiIcons.FiUser className="feather feather-phone me-3"></FiIcons.FiUser>
              )}
            </Link>
            {UserLocalStorage?.FullName ?
              <div className="user-greeting">
                שלום {UserLocalStorage.FullName}
              </div>
              :
              null
            }

          </div>
        </div>
      </div>
    );
  } else return <></>;
}