import logo from './logo.svg'
import './App.css'
import { useDecrementCountMutation, useGetCurrentCountQuery, useIncrementCountMutation } from './services/application'

function App() {
  const { data, isLoading } = useGetCurrentCountQuery();

  const [incrementCounter] = useIncrementCountMutation();
  const [decrementCounter] = useDecrementCountMutation();

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>Hello Vite + React! Vite's so cool!</p>
        <div>
          <div>
            { isLoading ? "Loading..." : data}
          </div>
          <div>
            <button type="button" onClick={() => incrementCounter()}>
              Increment
            </button>
            <button type="button" onClick={() => decrementCounter()}>
              Decrement
            </button>
          </div>
        </div>
        <p>
          Edit <code>App.tsx</code> and save to test HMR updates.
        </p>
        <p>
          <a
            className="App-link"
            href="https://reactjs.org"
            target="_blank"
            rel="noopener noreferrer"
          >
            Learn React
          </a>
          {' | '}
          <a
            className="App-link"
            href="https://vitejs.dev/guide/features.html"
            target="_blank"
            rel="noopener noreferrer"
          >
            Vite Docs
          </a>
        </p>
      </header>
    </div>
  )
}

export default App
