import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import * as z from 'zod'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Combobox } from '@/components/ui/combobox'
import { CalendarIcon, StepBackIcon, UserPlus2 } from 'lucide-react'
import { cn } from '@/lib/utils'
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover'
import { Calendar } from '@/components/ui/calendar'
import { formatDate } from '@/lib/format'
import { Textarea } from '@/components/ui/textarea'
import { Checkbox } from '@/components/ui/checkbox'
import customerService from '@/services/customer-service'
import { useToast } from '@/components/ui/use-toast'
import IFormRegister from '@/models/FormRegister.ts'
import { Link, useNavigate } from 'react-router-dom'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import { useState } from 'react'
import { log } from 'console'

const regexCaNhan = '/^[0-9]{10}$/'
const regexToChuc = '/[0-9]{10}/'

const formSchema = z.object({
  isPerson: z.string(),
  organizationName: z.string(),
  identityDate: z.date(),
  // identityNumberCaNhan: z.array(z.string().min(10)),
  // identityNumberCaNhan: z.string().refine(
  //   (v) => {
  //     let n = Number(v)
  //     return !isNaN(n) && v?.length > 0
  //   },
  //   { message: 'Invalid number' },
  // ),
  identityNumberCaNhan: z
    .number()
    .or(
      z
        .string()
        .regex(/\d+/, { message: 'Chỉ được nhập số' })
        .transform(Number),
    )
    .refine(
      (n) =>
        (n >= 100000000000 && n <= 999999999999) ||
        (n >= 100000000 && n <= 999999999),
      { message: 'Số CMT / Căn cước chỉ cho phép 9 hoặc 12 số' },
    )
    .optional(),

  identityNumberToChuc: z
    .number()
    .or(
      z
        .string()
        .regex(/\d+/, { message: 'Chỉ được nhập số' })
        .transform(Number),
    )
    .refine(
      (n) =>
        (n >= 1000000000000 && n <= 9999999999999) ||
        (n >= 1000000000 && n <= 9999999999),
      { message: 'Mã số thuế chỉ cho phép 10 hoặc 13 số' },
    )
    .optional(),
  // identityForm: z
  //   .object({
  //     identityNumber1: z
  //       .string()
  //       .refine((val) => !Number.isNaN(parseInt(val, 10)), {
  //         message: 'Chỉ được nhập số',
  //       }),
  //     isPerson1: z.string(),
  //   })
  //   .superRefine((val, ctx) => {
  //     if (val.isPerson1 == 'caNhan') {
  //       ctx.addIssue({
  //         code: z.ZodIssueCode.too_big,
  //         maximum: 3,
  //         type: 'array',
  //         inclusive: true,
  //         message: 'Too many items 😡',
  //       })
  //     } else {
  //       ctx.addIssue({
  //         code: z.ZodIssueCode.too_big,
  //         maximum: 4,
  //         type: 'array',
  //         inclusive: true,
  //         message: 'Too many items123123 😡',
  //       })
  //     }
  //   }),
  identityPlace: z.string(),
  city: z.string(),
  rePresentativeName: z.string(),
  roles: z.string(),
  perOrgName: z.string(),
  address: z.string(),
  engName: z.string(),
  engAddress: z.string(),
  phone: z.string(),
  fax: z.string(),
  email: z.string(),
  emailReceipt: z.string(),
  accountName: z.string(),
  password: z.string(),
  confirmPassword: z.string(),
  nationalAccountName: z.string(),
  nationalPassword: z.string(),
  items: z.string(),
})

const Register = () => {
  const { toast } = useToast()
  const options = [
    { label: 'Hà Nội', value: '0' },
    { label: 'Hải Phòng', value: '1' },
    { label: 'Thành phố Hồ Chí Minh', value: '2' },
    { label: 'Đà Nẵng', value: '3' },
  ]

  const navigate = useNavigate()

  const items = [
    {
      id: 'import',
      label: 'Kiểm dịch thực vật nhập khẩu',
    },
    {
      id: 'export',
      label: 'Kiểm dịch thực vật xuất khẩu, tái xuất',
    },
  ] as const

  // 1. Define your form.
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      isPerson: 'caNhan',
      organizationName: '',
      identityPlace: '',
      identityDate: new Date('1900-01-01'),
      city: '',
      engAddress: '',
      engName: '',
      fax: '',
      emailReceipt: '',
      nationalAccountName: '',
      nationalPassword: '',
      items: 'NK',
    },
  })

  const [isPerson, setIsPerson] = useState('')

  function handleChangeRadio(value: string) {
    console.log(value)
    setIsPerson(value)
  }
  function handleFileChange(event: React.ChangeEvent<HTMLInputElement>) {
    console.log(event?.target?.value)
  }

  // 2. Define a submit handler.
  function onSubmit(values: z.infer<typeof formSchema>) {
    const data: IFormRegister = {
      isPerson: values.isPerson === 'caNhan',
      cus_person: values.organizationName,
      cus_so_dk:
        values.isPerson === 'caNhan'
          ? values.identityNumberCaNhan?.toString()
          : values.identityNumberToChuc?.toString(),
      // cus_so_dk: values.identityForm,
      cus_country_code: values.city,
      cus_ngay_cap: values.identityDate,
      cus_noi_cap: values.identityPlace,
      nguoi_dai_dien: values.rePresentativeName,
      chuc_vu: values.roles,
      cus_ten_chu_hang: values.perOrgName,
      cus_dia_chi: values.address,
      customer_name: values.engName,
      cus_address: values.engAddress,
      cus_tel: values.phone,
      cus_email: values.email,
      cus_fax: values.fax,
      email_hoa_don: values.emailReceipt,
      lon_login_name: values.accountName,
      lon_login_password: values.password,
      confirm_password: values.confirmPassword,
      thu_tuc: values.items,
      account_1_cua: values.nationalAccountName,
      password_1_cua: values.nationalPassword,
    }
    console.log(data)
    customerService
      .create(data)
      .then((response: any) => {
        if (response.data) {
          toast({
            variant: 'destructive',
            title: 'Thành công',
            description: 'Đăng kí tài khoản thành công',
          })
          form.reset()
          navigate('/')
        }
      })
      .catch((e: Error) => {
        console.log(e)
        toast({
          variant: 'destructive',
          title: 'Thất bại',
          description: 'Đăng ký không thành công',
        })
      })
  }

  return (
    <div
      className="h-full flex justify-center bg-center  px-4 sm:px-6 lg:px-8  bg-no-repeat bg-cover items-center"
      style={{
        backgroundImage: `url("https://www.freecodecamp.org/news/content/images/2021/06/w-qjCHPZbeXCQ-unsplash.jpg")`,
      }}
    >
      <div className="max-w-3xl w-full p-10 bg-white rounded-xl shadow-lg z-10 m-auto flex flex-col mt-8 mb-8">
        <h2 className="font-semibold text-lg text-center">
          PQS - Đăng ký tài khoản {'('}Tổ chức / cá nhân{')'}
        </h2>
        <div className="flex text-sm italic text-red-500 mt-4">
          <p>Ghi chú:</p>
          <div className="flex flex-col ml-1">
            <p>
              - Đăng ký tài khoản cá nhân chụp CMT hoặc CCCD gửi đến zalo:
              0912471508
            </p>
            <p>
              - Đăng ký tài khoản cá nhân dưới danh nghĩa tổ chức {'('}công ty,
              doanh nghiệp{')'} sẽ bị khoá tài khoản
            </p>
            <div></div>
          </div>
        </div>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-2">
            <fieldset className="flex border border-solid border-sky-300 p-3 mt-5 rounded-md">
              <legend className="text-sm bg-sky-200 p-1 rounded-md">
                ĐIỀN THÔNG TIN
              </legend>
              <div className="flex flex-col w-full">
                <FormField
                  control={form.control}
                  name="isPerson"
                  render={({ field }) => (
                    <FormItem
                      className="space-y-3"
                      onChange={() => handleChangeRadio(field.value)}
                    >
                      <FormControl>
                        <RadioGroup
                          onValueChange={field.onChange}
                          defaultValue="caNhan"
                          className="flex flex-col space-y-1"
                        >
                          <FormItem className="flex items-center space-x-3 space-y-0">
                            <FormControl>
                              <RadioGroupItem value="toChuc" />
                            </FormControl>
                            <FormLabel className="font-normal">
                              Tổ chức
                            </FormLabel>
                          </FormItem>
                          <FormItem className="flex items-center space-x-3 space-y-0">
                            <FormControl>
                              <RadioGroupItem value="caNhan" />
                            </FormControl>
                            <FormLabel className="font-normal">
                              Cá nhân
                            </FormLabel>
                          </FormItem>
                        </RadioGroup>
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                ></FormField>
                <br />
                <div className="flex">
                  <div className="basis-1/3 mr-3">
                    <FormField
                      control={form.control}
                      name="organizationName"
                      render={({ field }) => (
                        <FormItem>
                          {isPerson === 'caNhan' ? (
                            <FormLabel>Tên tổ chức </FormLabel>
                          ) : (
                            <FormLabel>Tên cá nhân </FormLabel>
                          )}
                          <abbr title="required" className="text-red-500">
                            *
                          </abbr>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  <div className="basis-2/3">
                    <FormField
                      control={form.control}
                      name="city"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Thuộc tỉnh / thành phố </FormLabel>
                          <abbr title="required" className="text-red-500">
                            *
                          </abbr>
                          <FormControl>
                            <Combobox options={options} {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                </div>
                <div className="flex flex-row mt-2">
                  <div className="basis-1/3 mr-3">
                    {isPerson === 'caNhan' ? (
                      <FormField
                        control={form.control}
                        name="identityNumberToChuc"
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Mã số thuế </FormLabel>
                            <FormControl>
                              <Input type="text" {...field} />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                    ) : (
                      <FormField
                        control={form.control}
                        name="identityNumberCaNhan"
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Số CMT / Căn cước </FormLabel>
                            <FormControl>
                              <Input type="text" {...field} />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                    )}
                  </div>
                  <div className="basis-1/3 mr-3">
                    <FormField
                      control={form.control}
                      name="identityDate"
                      render={({ field }) => (
                        <FormItem className="flex flex-col mt-[0.6rem]">
                          <FormLabel>Ngày cấp </FormLabel>
                          <Popover style={{ marginTop: 1 }}>
                            <PopoverTrigger asChild>
                              <FormControl>
                                <Button
                                  variant={'outline'}
                                  className={cn(
                                    'w-full pl-3 text-left font-normal',
                                    !field.value && 'text-muted-foreground',
                                  )}
                                >
                                  {field.value != new Date('1900-01-01') ? (
                                    formatDate(field.value, 'en-GB')
                                  ) : (
                                    <span>Chọn ngày</span>
                                  )}
                                  <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                </Button>
                              </FormControl>
                            </PopoverTrigger>
                            <PopoverContent
                              className="w-auto p-0"
                              align="start"
                            >
                              <Calendar
                                mode="single"
                                selected={field.value}
                                onSelect={field.onChange}
                                disabled={(date) =>
                                  date > new Date() ||
                                  date < new Date('1900-01-01')
                                }
                                initialFocus
                              />
                            </PopoverContent>
                          </Popover>

                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  <div className="basis-1/3">
                    {' '}
                    <FormField
                      control={form.control}
                      name="identityPlace"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Nơi cấp </FormLabel>
                          <abbr title="required" className="text-red-500">
                            *
                          </abbr>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                </div>
              </div>
            </fieldset>

            <fieldset className="flex border border-solid border-sky-300 p-3 mt-10 rounded-md">
              <legend className="text-sm bg-sky-200 p-1 rounded-md">
                THÔNG TIN TỔ CHỨC / CÁ NHÂN
              </legend>
              <div className="flex flex-col w-full">
                <div className="flex">
                  <div className="basis-1/2 mr-3">
                    <FormField
                      control={form.control}
                      name="rePresentativeName"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className={cn('mr-1')}>
                            Người đại diện pháp luật
                          </FormLabel>
                          <abbr title="required" className="text-red-500">
                            *
                          </abbr>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  <div className="basis-1/2">
                    <FormField
                      control={form.control}
                      name="roles"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className={cn('mr-1')}>Chức vụ</FormLabel>
                          <abbr title="required" className="text-red-500">
                            *
                          </abbr>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                </div>
                <div className="mt-2">
                  <FormField
                    control={form.control}
                    name="perOrgName"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className={cn('mr-1')}>
                          Tên tổ chức / cá nhân
                        </FormLabel>
                        <abbr title="required" className="text-red-500">
                          *
                        </abbr>
                        <FormControl>
                          <Textarea className="resize-none" {...field} />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>
                <div className="mt-2">
                  <FormField
                    control={form.control}
                    name="address"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className={cn('mr-1')}>Địa chỉ</FormLabel>
                        <abbr title="required" className="text-red-500">
                          *
                        </abbr>
                        <FormControl>
                          <Textarea className="resize-none" {...field} />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>
                <div className="mt-2">
                  <FormField
                    control={form.control}
                    name="engName"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Tên tiếng Anh</FormLabel>
                        <FormControl>
                          <Textarea className="resize-none" {...field} />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>
                <div className="mt-2">
                  <FormField
                    control={form.control}
                    name="engAddress"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Địa chỉ tiếng Anh</FormLabel>
                        <FormControl>
                          <Textarea className="resize-none" {...field} />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>

                <div className="flex mt-2">
                  <div className="basis-1/2 mr-3">
                    <FormField
                      control={form.control}
                      name="phone"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className={cn('mr-1')}>
                            Điện thoại
                          </FormLabel>
                          <abbr title="required" className="text-red-500">
                            *
                          </abbr>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  <div className="basis-1/2">
                    <FormField
                      control={form.control}
                      name="fax"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Fax</FormLabel>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                </div>

                <div className="flex mt-2">
                  <div className="basis-1/2 mr-3">
                    <FormField
                      control={form.control}
                      name="email"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className={cn('mr-1')}>
                            Email đăng ký
                          </FormLabel>
                          <abbr title="required" className="text-red-500">
                            *
                          </abbr>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  <div className="basis-1/2">
                    <FormField
                      control={form.control}
                      name="emailReceipt"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel>Email nhận hoá đơn</FormLabel>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                </div>
              </div>
            </fieldset>

            <fieldset className="flex border border-solid border-sky-300 p-3 mt-10 rounded-md">
              <legend className="text-sm bg-sky-200 p-1 rounded-md">
                MẬT KHẨU ĐĂNG NHẬP VÀO HỆ THỐNG
              </legend>
              <div className="flex flex-row w-full items-center">
                <div className="basis-1/3 mr-[150px] ">
                  <FormField
                    control={form.control}
                    name="accountName"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className={cn('mr-1')}>
                          Tên tài khoản
                        </FormLabel>
                        <abbr title="required" className="text-red-500">
                          *
                        </abbr>

                        <FormControl>
                          <Input {...field} />
                        </FormControl>

                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>
                <div className="flex-col basis-1/2">
                  <div className="basis-1/3 mr-3">
                    <FormField
                      control={form.control}
                      name="password"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className={cn('mr-1')}>
                            Mật khẩu{' '}
                          </FormLabel>
                          <abbr title="required" className="text-red-500">
                            *
                          </abbr>
                          <FormControl>
                            <Input type={'password'} {...field} />
                          </FormControl>

                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  <div className="basis-2/3">
                    <FormField
                      control={form.control}
                      name="confirmPassword"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className={cn('mr-1')}>
                            Nhập lại mật khẩu
                          </FormLabel>
                          <abbr title="required" className="text-red-500">
                            *
                          </abbr>
                          <FormControl>
                            <Input type={'password'} {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                </div>
              </div>
            </fieldset>

            <div className="flex">
              <fieldset className="flex basis-1/2 mr-3 border border-solid border-sky-300 p-5 rounded-md">
                <legend className="text-sm bg-sky-200 p-1 rounded-md">
                  KHAI BÁO THỦ TỤC
                </legend>
                <div className="flex flex-col w-full">
                  <FormField
                    control={form.control}
                    name="items"
                    render={({ field }) => (
                      <FormItem className="space-y-3">
                        <FormControl>
                          <RadioGroup
                            onValueChange={field.onChange}
                            defaultValue="NK"
                            className="flex flex-col space-y-1"
                          >
                            <FormItem className="flex items-center space-x-3 space-y-0">
                              <FormControl>
                                <RadioGroupItem value="NK" />
                              </FormControl>
                              <FormLabel className="font-normal">
                                Kiểm dịch thực vật nhập khẩu
                              </FormLabel>
                            </FormItem>
                            <FormItem className="flex items-center space-x-3 space-y-0">
                              <FormControl>
                                <RadioGroupItem value="XK" />
                              </FormControl>
                              <FormLabel className="font-normal">
                                Kiểm dịch thực vật xuất khẩu, tái xuất
                              </FormLabel>
                            </FormItem>
                          </RadioGroup>
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  ></FormField>
                </div>
              </fieldset>

              <fieldset className="flex basis-1/2 border border-solid border-sky-300 p-3 rounded-md">
                <legend className="text-sm  bg-sky-200 p-1 rounded-md">
                  TÀI KHOẢN TRÊN MỘT CỬA QUỐC GIA
                </legend>
                <div className="flex flex-col w-full">
                  <div>
                    <FormField
                      control={form.control}
                      name="nationalAccountName"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className={cn('mr-1')}>
                            Tên đăng nhập
                          </FormLabel>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                  </div>
                  <div className="flex flex-col w-full">
                    <div className="basis-4/5">
                      <FormField
                        control={form.control}
                        name="nationalPassword"
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel className={cn('mr-1')}>
                              Mật khẩu
                            </FormLabel>
                            <FormControl>
                              <Input type={'password'} {...field} />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                    </div>
                  </div>
                </div>
              </fieldset>
            </div>
            <div className="flex">
              <div className="flex basis-1/5 ">
                <button
                  type="submit"
                  className="text-white bg-green-400 hover:bg-green-500 border border-gray-200 focus:ring-4 focus:outline-none  focus:ring-gray-100 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:focus:ring-gray-600 dark:bg-gray-800 dark:border-gray-700 dark:text-white dark:hover:bg-gray-700 mr-2 mb-2"
                >
                  <UserPlus2 />
                  <span className="ml-2">Đăng ký</span>
                </button>
              </div>

              <Link className="ml-72" to="/">
                <button className="text-white bg-blue-400 hover:bg-green-500 border border-gray-200 focus:ring-4 focus:outline-none  focus:ring-gray-100 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:focus:ring-gray-600 dark:bg-gray-800 dark:border-gray-700 dark:text-white dark:hover:bg-gray-700 mr-2 mb-2">
                  <StepBackIcon />
                  <span className="ml-2">Quay lại trang đăng nhập</span>
                </button>
              </Link>
            </div>
            <div className="flex basis-3/4 items-center m-auto">
              <span className="text-sm italic text-red-500">
                Ghi chú: Hồ sơ của tổ chức / cá nhân đã tồn tại trong hệ thống
                thì nút Đăng ký sẽ bị mờ !
              </span>
            </div>
          </form>
        </Form>
      </div>
    </div>
  )
}

export default Register
