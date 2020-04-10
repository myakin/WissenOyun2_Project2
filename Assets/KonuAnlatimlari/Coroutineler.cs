using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutineler : MonoBehaviour {
    // Coroutine'ler ---> Konu Anlatimi +++++++++++++++++++++++++++++
    // 1. ornek
    private bool isMyJobCoroutineActive;
    public void ExecuteMyJob() {
        if (!isMyJobCoroutineActive) {
            isMyJobCoroutineActive = true;
            StartCoroutine(DoMyJobAfterDelay());
        }
    }
    private IEnumerator DoMyJobAfterDelay() {
        yield return new WaitForSeconds(10f);
        MyJob();
        isMyJobCoroutineActive = false;
    }
    private void MyJob() {
        // ne yapmak istiyorsaniz
    }


    // 2. ornek
    private bool isCoroutineActive;
    int seconds = 0;
    public void StartMyCoroutine() {
        if (!isCoroutineActive) {
            isCoroutineActive=true;
            StartCoroutine(MyExampleCoroutine());
        }
    }
    private IEnumerator MyExampleCoroutine() {
        yield return new WaitForSeconds(1f);
        seconds++;
        if (seconds>10) {
            StopCoroutine(MyExampleCoroutine());
            isCoroutineActive = false;
        } else {
            yield return MyExampleCoroutine();
        }
    }


    // 3. ornek (1. ornegin ikinci yaklasimla ele alinmasi)
    private IEnumerator myCoroutineRecord;

    public void StartDoMyOtherJob() {
        if (myCoroutineRecord==null) {
            myCoroutineRecord = DoMyOtherJobAfterDelay();
            StartCoroutine(myCoroutineRecord);
        }
    }
    private IEnumerator DoMyOtherJobAfterDelay() {
        yield return new WaitForSeconds(10f);
        MyOtherJob();
        myCoroutineRecord = null;
    }
    private void MyOtherJob() {
        // ne yapmak istiyorsaniz
    }

    // 4. ornek (2. ornegin ikinci yaklasimla ele alinmasi)
    private IEnumerator myCoroutineRecord2;
    int seconds2 = 0;
    public void StartMyCoroutine2() {
        if (myCoroutineRecord2==null) {
            myCoroutineRecord2 = MyExampleCoroutine2();
            StartCoroutine(myCoroutineRecord2);
        }
    }
    private IEnumerator MyExampleCoroutine2() {
        yield return new WaitForSeconds(1f);
        seconds++;
        if (seconds<=10) {
            yield return MyExampleCoroutine2();
        }
    }
    public void StopMyExampleCoroutine2() {
        StopCoroutine(myCoroutineRecord2);
    }

    // 5. ornek (parametre gecme)
    private IEnumerator myCoroutineRecord5;

    public void StartDoMyOtherJob5() {
        if (myCoroutineRecord5==null) {
            myCoroutineRecord5 = DoMyOtherJobAfterDelay5(15f);
            StartCoroutine(myCoroutineRecord5);
        }
    }
    private IEnumerator DoMyOtherJobAfterDelay5(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        MyOtherJob5();
        myCoroutineRecord = null;
        //yield return DoMyOtherJobAfterDelay5(waitTime); // gerekseydi kendini bu sekilde cagiracak idi
    }
    private void MyOtherJob5() {
        // ne yapmak istiyorsaniz
    }

    // Coroutine'ler ---> Konu Anlatimi -------------------------------
}
