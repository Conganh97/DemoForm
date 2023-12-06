import {zodResolver} from '@hookform/resolvers/zod'
import {useForm} from 'react-hook-form'
import * as z from 'zod'
import {Form, FormControl, FormField, FormItem, FormLabel,} from '@/components/ui/form'
import {Input} from '@/components/ui/input'
import {User2, UserPlus} from 'lucide-react'
import customerService from '@/services/customer-service'
import {useToast} from '@/components/ui/use-toast'
import IFormLogin from "@/models/FormLogin.ts";
import {cn} from "@/lib/utils.ts";
import {Link, useNavigate} from "react-router-dom";

const loginForm = z.object({
    userName: z.string(),
    password: z.string(),
})


const Login = () => {
    const {toast} = useToast()
    // 1. Define your form.
    const form = useForm<z.infer<typeof loginForm>>({
        resolver: zodResolver(loginForm),
        defaultValues: {
            userName: '',
            password: '',
        }
    })

    const navigate = useNavigate();

    // 2. Define a submit handler.
    function onSubmit(values: z.infer<typeof loginForm>) {
        const data: IFormLogin = {
            userName: values.userName,
            password: values.password,
        }

        customerService
            .login(data)
            .then((response) => {
                if (response.data != "") {
                    localStorage.setItem("user", JSON.stringify(response.data))
                    toast({
                        variant: "destructive",
                        title: 'Thành công',
                        description: 'Đăng nhập thành công',
                    })
                    navigate("/home")
                } else {
                    toast({
                        variant: 'destructive',
                        title: 'Đăng Nhập Thất bại',
                        description: 'Vui lòng kiểm tra lại thông tin tài khoản',
                    })
                }
                form.reset()
            })
            .catch((e: Error) => {
                console.log(e)
            })
    }

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
            <div className="max-w-lg w-full p-10 bg-white rounded-xl shadow-lg z-10 m-auto flex flex-col mt-8 mb-8">
                <h2 className="font-semibold text-lg text-center">
                    PQS - Đăng nhập vào trang khai báo
                </h2>

                <Form {...form}>
                    <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-2">
                        <fieldset className="flex border border-solid border-sky-300 p-3 mt-5 rounded-md">
                            <legend className="text-sm bg-sky-200 p-1 rounded-md">
                                ĐĂNG NHẬP
                            </legend>
                            <div className="flex flex-col w-full">
                                <div className="flex-col">
                                    <div>
                                        <FormField
                                            control={form.control}
                                            name="userName"
                                            render={({field}) => (
                                                <FormItem>
                                                    <FormLabel className={cn('mr-1')}>
                                                        Tên tài khoản:
                                                    </FormLabel>
                                                    <FormControl>
                                                        <Input required={true} {...field} />
                                                    </FormControl>
                                                </FormItem>
                                            )}
                                        />
                                    </div>
                                    <div>
                                        <FormField
                                            control={form.control}
                                            name="password"
                                            render={({field}) => (
                                                <FormItem>
                                                    <FormLabel className={cn('mr-1')}>Mật khẩu:</FormLabel>
                                                    <FormControl>
                                                        <Input required={true} type={"password"} {...field} />
                                                    </FormControl>
                                                </FormItem>
                                            )}
                                        />
                                    </div>
                                </div>
                            </div>
                        </fieldset>

                        <div className="flex basis-12/12 justify-end">
                            <button
                                type="submit"
                                className="text-white bg-green-400 hover:bg-green-500 border border-gray-200 focus:ring-4 focus:outline-none  focus:ring-gray-100 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:focus:ring-gray-600 dark:bg-gray-800 dark:border-gray-700 dark:text-white dark:hover:bg-gray-700 mr-2 mb-2"
                            >
                                <User2/>
                                <span className="ml-2">Đăng Nhập</span>
                            </button>
                            <Link className="ml-5" to="/register">
                                <button
                                    className="text-white bg-blue-400 hover:bg-green-500 border border-gray-200 focus:ring-4 focus:outline-none  focus:ring-gray-100 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:focus:ring-gray-600 dark:bg-gray-800 dark:border-gray-700 dark:text-white dark:hover:bg-gray-700 mr-2 mb-2"
                                >
                                    <UserPlus/>
                                    <span className="ml-2">Đăng Ký</span>
                                </button>
                            </Link>
                        </div>
                    </form>
                </Form>
            </div>
        </div>
    )
}
export default Login
