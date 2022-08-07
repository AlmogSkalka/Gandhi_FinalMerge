import React, { useState, useEffect } from "react";
import { Row, Col, Button, Card, Container } from "react-bootstrap";
import { Spinner } from "react-bootstrap";
import SearchList from "../Comps/Internal Comps/Search/SearchList";
import "react-multi-carousel/lib/styles.css";

const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";

export default function PersonalArea() {
  const [BoughtHereItems, setBoughtHereItems] = useState([]);
  const [LikedItems, setLikedItems] = useState([]);
  const [UserCurrentSellingItems, setCurrentSellingItems] = useState([]);
  const [UserSoldItems, setUserSoldItems] = useState([]);
  const [CurrentSellingItemsLoading, setCurrentSellingItemsLoading] =
    useState(true);
  const [BoughtHereItemsLoading, setBoughtHereItemsLoading] = useState(true);
  const [LikedItemsLoading, setLikedItemsLoading] = useState(true);
  const [isSellerSide, setIsSellerSide] = useState(false);
  const [isBuyerSide, setIsBuyerSide] = useState(false);
  const [InterestingItems, setInterestingItems] = useState([]);
  const [InterestingItemsLoading, setInterestingItemsLoading] = useState(true);

  const UserLocalStorage = JSON.parse(localStorage.getItem("user"));
  const userId = UserLocalStorage.UserId;
  useEffect(() => {
    //Get User's Liked Items List
    fetch(
      ApiUrl + "Users/GetUserLikedItems?userId=" + userId,
      {
        method: "GET",
        headers: new Headers({
          "Content-Type": "application/json; charset=UTF-8",
          Accept: "application/json; charset=UTF-8",
        }),
      }
    )
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          setLikedItems(result);
          setLikedItemsLoading(false);
        },
        (error) => {}
      );

    //Get User's Sold Items List
    fetch(ApiUrl + "Users/GetUserSoldItems?userId=" + userId, {
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
          setUserSoldItems(result);
          setCurrentSellingItemsLoading(false);
        },
        (error) => {}
      );

    //Get User's Current On Sale Items List
    fetch(ApiUrl + "Users/GetUserSellItems?userId=" + userId, {
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
          setCurrentSellingItems(result);
          setCurrentSellingItemsLoading(false);
        },
        (error) => {}
      );
    //Get user's items that he baught in the app
    fetch(ApiUrl + "Users?userId=" + userId, {
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
          setBoughtHereItems(result);
          setBoughtHereItemsLoading(false);
        },
        (error) => {}
      );
    //Get User's Intersted Items List
    fetch(
      ApiUrl + "Users/GetUserInterstedItems?userId=" + userId,
      {
        method: "GET",
        headers: new Headers({
          "Content-Type": "application/json; charset=UTF-8",
          Accept: "application/json; charset=UTF-8",
        }),
      }
    )
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          setInterestingItems(result);
          setInterestingItemsLoading(false);
        },
        (error) => {}
      );
  }, [userId]);

  const ShowSellerSide = () => {
    setIsSellerSide(true);
    setIsBuyerSide(false);
  };

  const ShowBuyerSide = () => {
    setIsBuyerSide(true);
    setIsSellerSide(false);
  };

  return (
    <Container style={{ paddingTop: "10em" }}>
      {/*Header */}
      <Row>
        <Card
          className="bg-none"
          style={{ borderRadius: "5em", width: "12em" }}
        >
          <Card.Img
            variant="top"
            style={{ borderRadius: "5em" }}
            src={UserLocalStorage.ProfilePicUrl}
          />
          <Card.Body>
            <Card.Title> </Card.Title>
            <Card.Title> {UserLocalStorage.FullName} </Card.Title>
          </Card.Body>
        </Card>
      </Row>
      {/*Page Render Buttons */}
      <div className="container" style={{ marginBottom: "0.5em" }}>
        <div className="row h-100 g-0 itemformRow">
          <div className="col-md-6 ">
            <Button
              className="btn btn-light RoundElements ItemFormBtn"
              onClick={ShowBuyerSide}
            >
              פעילות בתור קונה
            </Button>
          </div>
          <div className="col-md-6 ">
            <Button
              className="btn btn-light RoundElements ItemFormBtn"
              onClick={ShowSellerSide}
            >
              פעילות בתור מוכר
            </Button>
          </div>
        </div>
      </div>
      {/*BuyerSide Rendering */}
      {isBuyerSide ? (
        <>
          {InterestingItems?.length ? (
            <>
              {InterestingItemsLoading ? (
                <Spinner animation="border" />
              ) : (
                <>
                  <Row>
                    <Col>
                      <h4 style={{ margin: "0px auto" }}>
                        {" "}
                        פריטים שהתעניינת בהם
                      </h4>
                    </Col>
                  </Row>
                  <SearchList filteredItems={InterestingItems} />
                </>
              )}
            </>
          ) : (
            <h4> עדיין לא התעניינת בפריט</h4>
          )}

          {LikedItems?.length ? (
            <>
              {LikedItemsLoading ? (
                <Spinner animation="border" />
              ) : (
                <>
                  <Row style={{ margin: "0px auto" }}>
                    <Col>
                      <h4> פריטים שאהבת</h4>
                    </Col>
                  </Row>
                  <SearchList filteredItems={LikedItems} />
                </>
              )}
            </>
          ) : (
            <h4>לא אהבת שום פריט</h4>
          )}

          {BoughtHereItems?.length ? (
            <>
              {BoughtHereItemsLoading ? (
                <Spinner animation="border" />
              ) : (
                <>
                  <Row style={{ margin: "0px auto" }}>
                    <Col>
                      <h4> פריטים שקנית פה</h4>
                    </Col>
                  </Row>
                  <SearchList filteredItems={BoughtHereItems} />
                </>
              )}
            </>
          ) : (
            <h4>לא קנית כלום באפליקציה</h4>
          )}
        </>
      ) : null}

      {/*SellerSide Rendering */}
      {isSellerSide ? (
        UserCurrentSellingItems ? (
          <>
            {CurrentSellingItemsLoading ? (
              <Spinner animation="border" />
            ) : (
              <>
                <Row style={{ margin: "0px auto" }}>
                  <Col>
                    <h4> פריטים שהעלת למכירה</h4>
                  </Col>
                </Row>
                <SearchList filteredItems={UserCurrentSellingItems} />
              </>
            )}

            {UserSoldItems?.length ? (
              <>
                {LikedItemsLoading ? (
                  <Spinner animation="border" />
                ) : (
                  <>
                    <Row style={{ margin: "0px auto" }}>
                      <Col>
                        <h4> פריטים שמכרת</h4>
                      </Col>
                    </Row>
                    <SearchList filteredItems={UserSoldItems} />
                  </>
                )}
              </>
            ) : (
              <h4> עדיין לא מכרת פריט באפליקציה</h4>
            )}
          </>
        ) : null
      ) : null}
    </Container>
  );
}
