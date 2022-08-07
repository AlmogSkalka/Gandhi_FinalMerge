import React, { useState, useEffect } from "react";
import "react-multi-carousel/lib/styles.css";
import { useNavigate } from "react-router";
import AddNewItem from "../Comps/Internal Comps/Item/AddNweItemBtn";
import "react-responsive-carousel/lib/styles/carousel.min.css"; // requires a loader
import { Spinner } from "react-bootstrap";
import SearchList from "../Comps/Internal Comps/Search/SearchList";

// const ApiUrl = "https://localhost:44315/api/";
const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";

export default function MainDashboard() {
  const [MostViewedItemsList, setMostViewedItemsList] = useState([]);
  const [AlgoRecommendedItems, setAlgoRecommendedItems] = useState([]);
  const [Algo4RecommendedItems, setAlgo4RecommendedItems] = useState([]);
  const [ShowAllAlgoItems, setShowAllAlgoItems] = useState(false);
  const [ShowMostViewedItems, setShowMostViewedItems] = useState(false);
  const [loadingAlgo4RecommendedItems, setLoadingAlgo4RecommendedItems] =
    useState(true);
  const [loadingMostViewedItems, setLoadingMostViewedItems] = useState(true);
  const [FourMostViewedItems, setFourMostViewedItems] = useState([])
  const UserLocalStorage = JSON.parse(localStorage.getItem("user"));
  const fourItems = 4;
  const navigate = useNavigate();

  useEffect(() => {
    fetch(ApiUrl + "Items", {
      method: "POST",
      body: JSON.stringify(UserLocalStorage),
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
          //once going to production, we will stop calling AlgoFetch() here, and we will call LatestAlgoFetch() instead
          // AlgoFetch();
          LatestAlgoFetch();
        },
        (error) => {
          console.log(error);
        }
      );
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const LatestAlgoFetch = () => {
    let today = new Date();
    let birthDayCheck = Math.abs(
      (UserLocalStorage.DateOfBirth - today.getTime()) / (1000 * 60 * 60 * 24)
    );
    //if user got birthday, so we tell him congrats
    if (birthDayCheck <= 0) {
      alert("יום הולדת שמח!!!!");
    }
    //if there's no field in localstorage for the latest update date, then set the date, and get algo items.
    if (!localStorage.getItem("algoUpdateDate")) {
      let algoUpdate = {
        LastUpdate: today.getTime(),
      };
      localStorage.setItem("algoUpdateDate", JSON.stringify(algoUpdate));
      AlgoFetch();
    }
    //if the last algo update date exist
    else if (localStorage.getItem("algoUpdateDate")) {
      //get the date
      let clientAlgoUpdate = JSON.parse(localStorage.getItem("algoUpdateDate"));
      //calculate the gap between the last update and today
      let updatesGap = Math.abs(
        (clientAlgoUpdate.LastUpdate - today.getTime()) / (1000 * 60 * 60 * 24)
      );
      //if gap is larger then a day, get  algo items from DB
      if (updatesGap >= 1) {
        AlgoFetch();
        let algoUpdate = {
          LastUpdate: today.getTime(),
        };
        localStorage.setItem("algoUpdateDate", JSON.stringify(algoUpdate));
      }
      //if gap is no larger then a day, get the items from localstorage and set it in the page itself instead of GET command to DB
      else {
        setMostViewedItemsList(
          JSON.parse(localStorage.getItem("LastUpdatedMostViewedItemsList"))
        );
        let tmpMostViewedArr = [];
        let localStorageMostViewedItems = JSON.parse(localStorage.getItem("LastUpdatedMostViewedItemsList"))
        for (let i = 0; i < Math.min(localStorageMostViewedItems.length, fourItems); i++) {
          tmpMostViewedArr.push(localStorageMostViewedItems[i])
        }
        setFourMostViewedItems(tmpMostViewedArr)
        setAlgoRecommendedItems(
          JSON.parse(localStorage.getItem("LastUpdatedRecommendedItems"))
        );
        let tmp4arr = [];
        let localStorageAlgoItems = JSON.parse(localStorage.getItem("LastUpdatedRecommendedItems"))
        for (let i = 0; i < Math.min(localStorageAlgoItems.length, fourItems); i++) {
          tmp4arr.push(localStorageAlgoItems[i]);
        }
        setAlgo4RecommendedItems(tmp4arr);
        setLoadingAlgo4RecommendedItems(false);
        setLoadingMostViewedItems(false);
      }
    }
  };

  const AlgoFetch = () => {
    fetch(ApiUrl + "PersonalCostomization?userId=" + UserLocalStorage.UserId, {
      method: "POST",
      body: JSON.stringify(UserLocalStorage),
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
          setMostViewedItemsList(result);
          let tmpMostViewedArr = [];
          for (let i = 0; i < Math.min(result.length, fourItems); i++) {
            tmpMostViewedArr.push(result[i])
          }
          setFourMostViewedItems(tmpMostViewedArr)
          setLoadingMostViewedItems(false);
          setLoadingAlgo4RecommendedItems(false);
          localStorage.setItem(
            "LastUpdatedMostViewedItemsList",
            JSON.stringify(result)
          );
        },
        (error) => {
          console.log(error);
        }
      );
    GetAlgoItems();
  };

  const GetAlgoItems = () => {
    fetch(ApiUrl + "PersonalCostomization", {
      method: "POST",
      body: JSON.stringify(UserLocalStorage),
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
          console.log("algo items: ", result)
          let tmparr = [];
          let tmp4arr = [];
          result.forEach((element) => {
            tmparr.push(element);
          });
          for (let index = 0; index < Math.min(result.length, fourItems); index++) {
            tmp4arr.push(result[index]);
          }
          setAlgoRecommendedItems(tmparr);
          setAlgo4RecommendedItems(tmp4arr);
          setLoadingAlgo4RecommendedItems(false);
          localStorage.setItem(
            "LastUpdatedRecommendedItems",
            JSON.stringify(result)
          );
        },
        (error) => {
          console.log(error);
        }
      );
  };

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

  return (
    <main className="main" id="top">
      <AddNewItem />
      <section className="py-11 bg-light-gradient border-bottom border-white border-5">
        <div
          className="bg-holder overlay overlay-light"
          style={{
            backgroundImage: "url(assets/img/gallery/header-bg.png)",
            backgroundSize: "cover",
          }}
        ></div>
        <div className="container">
          <div className="row flex-center">
            <div className="col-12 mb-10">
              <div className="d-flex align-items-center flex-column">
                <h1 className="fw-normal"> גם עם טעם ייחודי כמו שלך</h1>
                <h1 className="fs-4 fs-lg-8 fs-md-6 fw-bold">
                  אנחנו בטוחים שתמצאו פה את הפריט המתאים עבורכם
                </h1>
              </div>
            </div>
          </div>
        </div>
      </section>
      <section className="py-0" id="header">
        <div className="container">
          <div className="row g-0">
            <div onClick={() => CategoriesSearchPage(1)} className="col-md-4">
              <div className="card card-span h-100 text-white">
                <img
                  className="img-fluid"
                  src="assets/img/gallery/her.png"
                  width="790"
                  alt="..."
                />
                <div className="card-img-overlay d-flex flex-center">
                  {" "}
                  <p className="btn btn-lg btn-light">
                    בשבילה{" "}
                  </p>
                </div>
              </div>
            </div>
            <div onClick={() => CategoriesSearchPage(2)} className="col-md-4">
              <div className="card card-span h-100 text-white">
                {" "}
                <img
                  className="img-fluid"
                  src="assets/img/gallery/him.png"
                  width="790"
                  alt="..."
                />
                <div className="card-img-overlay d-flex flex-center">
                  {" "}
                  <p className="btn btn-lg btn-light">
                    בשבילו{" "}
                  </p>
                </div>
              </div>
            </div>
            <div onClick={() => CategoriesSearchPage(3)} className="col-md-4">
              <div className="card card-span h-100 text-white">
                {" "}
                <img
                  className="img-fluid"
                  src="assets/img/gallery/they.png"
                  width="790"
                  alt="..."
                />
                <div className="card-img-overlay d-flex flex-center">
                  {" "}
                  <p className="btn btn-lg btn-light">
                    בשבילם{" "}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
      <section className="py-0">
        <div className="container">
          <div className="row h-100">
            <div className="col-lg-7 mx-auto text-center mt-7 mb-5">
              <h5 className="fw-bold fs-3 fs-lg-5 lh-sm">מותאם לך אישית</h5>
            </div>
            <div className="col-12">
              <div
                className="carousel slide"
                id="carouselBestDeals"
                data-bs-touch="false"
                data-bs-interval="false"
              >
                <div className="carousel-inner">
                  <div
                    className="carousel-item active"
                    data-bs-interval="10000"
                  >
                    {/* Algo4RecommendedItems */}
                    <div className="row h-100 align-items-center g-2">
                      {!loadingAlgo4RecommendedItems ? (
                        !ShowAllAlgoItems ? (
                          Algo4RecommendedItems ? (
                            <SearchList filteredItems={Algo4RecommendedItems} />
                          ) : (
                            <p>
                              תצטרכו לבלות קצת באפליקציה כדי שהאלגוריתם שלנו
                              ילמד אתכם!
                            </p>
                          )
                        ) : null
                      ) : (
                        <Spinner
                          animation="border"
                          style={{ margin: "0px auto", marginTop: "2rem" }}
                        />
                      )}
                      {!loadingAlgo4RecommendedItems ? (
                        <div className="col-12 d-flex justify-content-center mt-5">
                          <button
                            onClick={() =>
                              setShowAllAlgoItems(!ShowAllAlgoItems)
                            }
                            className="btn btn-lg btn-light"
                          >
                            {ShowAllAlgoItems
                              ? "הסתר את הפריטים"
                              : "הראה את כל הפריטים"}
                          </button>
                        </div>
                      ) : null}
                    </div>
                  </div>
                </div>
              </div>
            </div>
            {ShowAllAlgoItems ? (
              AlgoRecommendedItems ?
                <SearchList filteredItems={AlgoRecommendedItems} />
                :
                <p>תצטרכו לבלות קצת באפליקציה כדי שהאלגוריתם שלנו ילמד אתכם!</p>
            ) : null}
          </div>
        </div>
      </section>
      <section>
        <div className="container">
          <div className="row h-100 g-0">
            <div className="col-lg-7 mx-auto text-center mt-7 mb-5">
              <h5 className="fw-bold fs-3 fs-lg-5 lh-sm">
                הפריטים הנצפים ביותר
              </h5>
            </div>
            <div className="row h-100 align-items-center g-2">
              {!loadingMostViewedItems ? (
                !ShowMostViewedItems ? (
                  MostViewedItemsList ?
                    (
                      FourMostViewedItems ?
                        <SearchList filteredItems={FourMostViewedItems} />
                        :
                        "משהו קרה, אנחנו עובדים על זה!")
                    : (
                      <p>
                        תצטרכו לבלות קצת באפליקציה כדי שהאלגוריתם שלנו ילמד אתכם!
                      </p>
                    )
                ) : null
              ) : (
                <Spinner
                  animation="border"
                  style={{ margin: "0px auto", marginTop: "2rem" }}
                />
              )}
            </div>
          </div>
          <div className="col-12 d-flex justify-content-center mt-5">
            {!loadingMostViewedItems ? (
              <button
                disabled={loadingMostViewedItems}
                onClick={() => setShowMostViewedItems(!ShowMostViewedItems)}
                className="btn btn-lg btn-light"
              >
                {ShowMostViewedItems ? "הסתר את הפריטים" : "הראה את כל הפריטים"}
              </button>
            ) : null}
          </div>
          <div className="row h-100 align-items-center g-2">
            {ShowMostViewedItems ? (
              MostViewedItemsList ? (
                <SearchList filteredItems={MostViewedItemsList} />
              ) : (
                <p>משהו קרה, בד"כ פה יופיעו הפריטים הנצפים ביותר.</p>
              )
            ) : null}
          </div>
        </div>
      </section>
    </main>
  );
}
