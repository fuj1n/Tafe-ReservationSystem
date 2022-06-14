export default function ErrorDisplay({error, children}) {
    if (!error) {
        return children ?? (<></>);
    }

    if(error instanceof String || typeof error === 'string') {
        return <div className="text-danger">{error}</div>
    }

    return (
        <div>
            <span className="text-danger">{error.message}</span>
            {error.errors ? error.errors.map((error, index) => (
                <span key={index} className="text-danger">- {error.message}</span>
            )) : null}
        </div>
    );
}
