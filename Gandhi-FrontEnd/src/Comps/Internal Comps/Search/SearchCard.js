import React from "react";
import { useNavigate } from "react-router";

export default function SearchCard({ Item }) {
  const navigate = useNavigate();

  const Redirect2ItemPage = (itemObject) => {
    navigate("/Item", { state: itemObject });
  };

  return (
    <div className="search-card">
      <div>
        <img
          className="searchCards"
          src={Item.ItemPhotos[0]}
          style={{ width: "200px", margin: "0px auto", borderRadius: "0.5rem" }}
          onClick={() => Redirect2ItemPage(Item)}
          alt="תמונת פריט"
        />
        <div>
          <div>{Item.ItemDesc}</div>
          <p> מחיר - {Item.Price}</p>
        </div>
      </div>
    </div>
  );
}
