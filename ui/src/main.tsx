import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import { Provider } from "react-redux";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import App from "./App";
import Progress from "./IndefiniteProgress";
import DataInvalidator from "./dataInvalidator";

import { store } from "./store";

ReactDOM.render(
  <React.StrictMode>
    <Provider store={store}>
      <DataInvalidator>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<App />} />
            <Route path="/progressBar/:id" element={<Progress />} />
          </Routes>
        </BrowserRouter>
      </DataInvalidator>
    </Provider>
  </React.StrictMode>,
  document.getElementById("root")
);
