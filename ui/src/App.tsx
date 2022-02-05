import logo from './logo.svg'
import './App.css'
import { useDecrementCountMutation, useGetCurrentCountQuery, useIncrementCountMutation } from './services/application'

function App() {
  const { data, isLoading, refetch } = useGetCurrentCountQuery();

  const [incrementCounter] = useIncrementCountMutation();
  const [decrementCounter] = useDecrementCountMutation();

  const increment = async () => {
    await incrementCounter().then(() => refetch());
  }

  const decrement = async () => {
    await decrementCounter().then(() => refetch());
  }

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>Hello Vite + React! Vite's so cool!</p>
        <p>
          <div>
            { isLoading ? "Loading..." : data}
          </div>
          <div>
            <button type="button" onClick={() => increment()}>
              Increment
            </button>
            <button type="button" onClick={() => decrement()}>
              Decrement
            </button>
          </div>
        </p>
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
