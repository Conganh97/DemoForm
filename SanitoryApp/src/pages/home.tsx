import {User2} from 'lucide-react'
import {useNavigate} from "react-router-dom";

const Home = () => {

    function onSubmit() {
        localStorage.removeItem("user")
        navigate("/")
    }

    const navigate = useNavigate()

    const user = JSON.parse(localStorage.getItem("user"));
    if (user == null) {
        navigate("/")
    }

    const name = JSON.parse(localStorage.getItem("user"))?.loN_Login_name


    return (
        <div
            className="h-full flex justify-center bg-center px-4 sm:px-6 lg:px-8  bg-no-repeat bg-cover items-center"
            style={{
                backgroundPosition: 'center',
                backgroundSize: 'cover',
                backgroundRepeat: 'no-repeat',
                width: '100vw',
                height: '100vh',
                backgroundImage: `url("https://www.freecodecamp.org/news/content/images/2021/06/w-qjCHPZbeXCQ-unsplash.jpg")`,
            }}
        >
            <div className="max-w-3xl w-full p-10 bg-white rounded-xl shadow-lg z-10 m-auto flex flex-col mt-8 mb-8">
                <h2 className="font-semibold text-lg text-center">
                    Chào {name}!
                </h2>
                <br/>
                <div className="m-auto">
                    <button
                        type="button" onClick={onSubmit}
                        className="text-white bg-blue-400 hover:bg-green-500 border border-gray-200 focus:ring-4 focus:outline-none  focus:ring-gray-100 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:focus:ring-gray-600 dark:bg-gray-800 dark:border-gray-700 dark:text-white dark:hover:bg-gray-700 mr-2 mb-2"
                    >
                        <User2/>
                        <span className="ml-5">Đăng Xuất</span>
                    </button>
                </div>
            </div>

        </div>
    )
}
export default Home
