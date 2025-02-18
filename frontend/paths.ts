export default {
  home: "/",
  legalNotice: "/legal-notice",
  dataProtectionInformation: "/data-protection-information",
  databases: "/databases",
  institutions: "/institutions",
  institution(uuid: string) {
    return `/institutions/${encodeURIComponent(uuid)}`;
  },
  institutionCreate: "/institutions/create",
  users: "/users",
  user(uuid: string) {
    return `/users/${encodeURIComponent(uuid)}`;
  },
  userCurrent: "me",
  me: {
    manage: {
      profile: "/me/manage/profile",
      email: "/me/manage/email",
      changePassword: "/me/manage/change-password",
      setPassword: "/me/manage/set-password",
      twoFactorAuthentication: "/me/manage/two-factor-authentication",
      enableAuthenticator: "/me/manage/enable-authenticator",
      personalData: "/me/manage/personal-data",
    },
  },
  personalUserData: "/personal-user-data",
  userLogin: "/users/login",
  userRegister: "/users/register",
  userConfirmEmail: "/users/confirm-email",
  userForgotPassword: "/users/forgot-password",
  userLoginWithTwoFactorCode: "/users/login-with-two-factor-code",
  userLoginWithRecoveryCode: "/users/login-with-recovery-code",
  userCheckYourInbox: "/users/check-your-inbox",
  dataFormats: "/data-formats",
  dataFormat(uuid: string) {
    return `/data-formats/${encodeURIComponent(uuid)}`;
  },
  methods: "/methods",
  method(uuid: string) {
    return `/methods/${encodeURIComponent(uuid)}`;
  },
  components: "/components",
  component(uuid: string) {
    return `/components/${encodeURIComponent(uuid)}`;
  },
  openIdConnect: "/openIdConnect",
};
