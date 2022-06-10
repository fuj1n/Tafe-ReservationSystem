export default function Loader({loading, children}) {
    return loading ?
        <div className="d-flex w-100 justify-content-center">
            <div className="lds-dual-ring"/>
        </div> : children;
}
