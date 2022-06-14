using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SENAI_UC14_E4.Controllers;
using SENAI_UC14_E4.Interfaces;
using SENAI_UC14_E4.Models;
using SENAI_UC14_E4.ViewModels;

namespace TestXunitChapter.Controllers
{
    public class LoginControllerTest
    {
        //TESTE DE INTEGRAÇÃO 1 - RETORNO DE USUÁRIO INVÁLIDO
        [Fact]
        public void LoginController_Retornar_Usuario_Invalido()
        {
            //Arrange

            var fakeRepository = new Mock<IUsuarioRepository>();

            fakeRepository.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

            LoginViewModel dadosLogin = new LoginViewModel();

            dadosLogin.Email = "email@email.com";

            dadosLogin.Senha = "123";

            var controller = new LoginController(fakeRepository.Object);

            //Act

            var resultado = controller.Login(dadosLogin);

            //Assert

            Assert.IsType<UnauthorizedObjectResult>(resultado);
        }

        //TESTE DE INTEGRAÇÃO 2 - RETORNO DE TOKEN NO MÉTODO DE LOGIN
        [Fact]
        public void Login_Controller_Retornar_Token()
        {
            //Arrange

            Usuario usuarioRetorno = new Usuario();

            usuarioRetorno.Email = "email@email.com";

            usuarioRetorno.Senha = "1234";

            usuarioRetorno.Tipo = "0";

            var fakeRepository = new Mock<IUsuarioRepository>();

            fakeRepository.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(usuarioRetorno);

            string issuerValidacao = "chapter.webapi";

            LoginViewModel dadosLogin = new LoginViewModel();

            dadosLogin.Email = "email@email.com";

            dadosLogin.Senha = "1234";

            var controller = new LoginController(fakeRepository.Object);

            //Act

            OkObjectResult resultado = (OkObjectResult)controller.Login(dadosLogin);

            string token = resultado.Value.ToString().Split(' ')[3];

            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenJwt = jwtHandler.ReadJwtToken(token);

            //Assert

            Assert.Equal(issuerValidacao, tokenJwt.Issuer);
        }
    }
}
