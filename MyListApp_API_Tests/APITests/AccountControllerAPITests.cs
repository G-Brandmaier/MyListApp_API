﻿using RestSharp;
using System.Diagnostics.CodeAnalysis;

namespace MyListApp_API_Tests.APITests;

[ExcludeFromCodeCoverage]
public class AccountControllerAPITests
{
    private readonly RestClient _client;

    public AccountControllerAPITests()
    {
        _client = new RestClient("https://localhost:7223/api");
    }

}
