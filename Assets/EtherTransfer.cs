using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using UnityEngine;

public class EtherTransfer : MonoBehaviour {

    
    // Use this for initialization
    void Start () {

        StartCoroutine(TransferEther());
    }


    //Sample of new features / requests
    public IEnumerator TransferEther()
    {
        var url = "http://localhost:8545";
        var privateKey = "0xa5ca770c997e53e182c5015bcf1b58ba5cefe358bf217800d8ec7d64ca919edd";
        var account = "0x47E95DCdb798Bc315198138bC930758E6f399f81";
        //initialising the transaction request sender
        var ethTransfer = new EthTransferUnityRequest(url, privateKey, account);
        
        var receivingAddress = "0x09f8F8c219D94A5db8Ee466dC072748603A7A0D9";
        yield return ethTransfer.TransferEther("0x09f8F8c219D94A5db8Ee466dC072748603A7A0D9", 1.1m, 2);

        if (ethTransfer.Exception != null)
        {
            Debug.Log(ethTransfer.Exception.Message);
            yield break;
        }

        var transactionHash = ethTransfer.Result;

        Debug.Log("Transfer transaction hash:" + transactionHash);

        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);
        
        Debug.Log("Transaction mined");

        var balanceRequest = new EthGetBalanceUnityRequest(url);
        yield return balanceRequest.SendRequest(receivingAddress, BlockParameter.CreateLatest());
        
        
        Debug.Log("Balance of account:" + UnitConversion.Convert.FromWei(balanceRequest.Result.Value));
    }



    // Update is called once per frame
    void Update () {
		
	}
}
