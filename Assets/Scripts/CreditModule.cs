using System.Collections;
using System.Collections.Generic;
using GiraffeStar;
using UnityEngine;

public class ShowCreditSequenceMsg : MessageCore
{

}

public class CreditModule : Module {

    [Subscriber]
    void OnHandle(ShowCreditSequenceMsg msg)
    {
        
    }
}
