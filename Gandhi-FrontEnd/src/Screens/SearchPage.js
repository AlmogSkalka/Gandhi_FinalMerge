import React, { useState, useEffect } from "react";
import Search from "../Comps/Internal Comps/Search/Search";
import { Spinner } from "react-bootstrap";

const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";
// const ApiUrl = 'https://proj.ruppin.ac.il/igroup64/test2/tar6/api/'

// const geolib = require('geolib');

export default function SearchPage() {
  const [AllItemsList, setAllItemsList] = useState([]);
  const [Loading, setLoading] = useState(true);

  useEffect(() => {
    fetch(ApiUrl + "Items", {
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
          setAllItemsList(result);
          let tmpCategories = [];
          AllItemsList.forEach((element) => {
            tmpCategories.push(element.Category, element.DepartmentId);
          });
          setLoading(false);
        },
        (error) => {
          console.log(error);
        }
      );
      //eslint-disable-next-line react-hooks/exhaustive-deps
}, []);

  if (Loading) {
    return (
      <div style={{ paddingTop: "30em" }}>
        <Spinner animation="border" />
      </div>
    );
  } else if (!Loading) {
    return (
      <>
        <div className="searchpage">
          <Search details={AllItemsList} />
        </div>
      </>
    );
  }
}
