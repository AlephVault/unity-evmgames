﻿using AlephVault.Unity.EVMGames.Nethereum.Contracts.Constants;
using AlephVault.Unity.EVMGames.Nethereum.Contracts.Services;

namespace AlephVault.Unity.EVMGames.Nethereum.Contracts.Standards.ProofOfHumanity
{
    public class ProofOfHumanityService
    {
        private readonly IEthApiContractService _ethApiContractService;

        public ProofOfHumanityService(IEthApiContractService ethApiContractService)
        {
            _ethApiContractService = ethApiContractService;
        }

        public ProofOfHumanityContractService GetContractService(string contractAddress = CommonAddresses.PROOF_OF_HUMANITY_PROXY_ADDRESS)
        {
            return new ProofOfHumanityContractService(_ethApiContractService, contractAddress);
        }
    }

    public class IsRegisteredInfo
    {
        public string Address { get; set; }
        public bool IsRegistered { get; set; }
    }

}