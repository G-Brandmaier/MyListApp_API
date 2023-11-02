using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyListApp_API_Tests.APITests;

public class AccountControllerAPITests
{
    private readonly RestClient _client;

    public AccountControllerAPITests()
    {
        _client = new RestClient("https://localhost:7223/api");
    }

}
