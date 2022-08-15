# Asynchronous-Programming-csharp
This repository is used for practing asynchronous programming, including async, await mechanisms and task events in C#.

## Problem Description
- �M�׽Шϥ� library, �A�n�إߤ@�Ӥ�K��L�H�ϥΦۭq����A�Ȫ��禡�w

1. �إߤ@�����O�z�LHTTP�걵�ثe���ۭq����A��
	- Swagger:
	- �d�ҽШD:
```
POST http://192.168.10.146:5000/api/customreport
{
"dtno": 5493,
"ftno": 0,
"params": "AssignID=00878;AssignDate=20200101-99999999;DTOrder=2;MTPeriod=2;",
"keyMap": "",
"assignSpid": ""
}
�d�Ҧ^��:
{
"isCompleted": true,
"isFaulted": false,
"signature": "99.95",
"exception": "",
"result": "AAgFAAAAAQAAAP////8BAAAAAAAAAAwCAAAATlN5c3RlbS5EYXRhLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OQUBAAAAFVN5c3RlbS5EYXRhLkRhdGFUYWJsZQMAAAAZRGF0YVRhYmxlLlJlbW90aW5nVmVyc2lvbglYbWxTY2hlbWELWG1sRGlmZkdyYW0DAQEOU3lzdGVtLlZlcnNpb24CAAAACQMAAAAGBAAAANsGPD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTE2Ij8+DQo8eHM6c2NoZW1hIHhtbG5zPSIiIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6bXNkYXRhPSJ1cm46c2NoZW1hcy1taWNyb3NvZnQtY29tOnhtbC1tc2RhdGEiPg0KICA8eHM6ZWxlbWVudCBuYW1lPSJVbktub3ciPg0KICAgIDx4czpjb21wbGV4VHlwZT4NCiAgICAgIDx4czpzZXF1ZW5jZT4NCiAgICAgICAgPHhzOmVsZW1lbnQgbmFtZT0i5bm05pyIIiB0eXBlPSJ4czpzdHJpbmciIG1zZGF0YTp0YXJnZXROYW1lc3BhY2U9IiIgbWluT2NjdXJzPSIwIiAvPg0KICAgICAgICA8eHM6ZWxlbWVudCBuYW1lPSLlubTmnIhfeDAwM0Ff5pyf5bqVIiB0eXBlPSJ4czpzdHJpbmciIG1zZGF0YTp0YXJnZXROYW1lc3BhY2U9IiIgbWluT2NjdXJzPSIwIiAvPg0KICAgICAgICA8eHM6ZWxlbWVudCBuYW1lPSLmlLbnm6Tlg7lfeDAwM0Ff5pyf5bqVIiB0eXBlPSJ4czpkZWNpbWFsIiBtc2RhdGE6dGFyZ2V0TmFtZXNwYWNlPSIiIG1pbk9jY3Vycz0iMCIgLz4NCiAgICAgIDwveHM6c2VxdWVuY2U+DQogICAgPC94czpjb21wbGV4VHlwZT4NCiAgPC94czplbGVtZW50Pg0KICA8eHM6ZWxlbWVudCBuYW1lPSJ0bXBEYXRhU2V0IiBtc2RhdGE6SXNEYXRhU2V0PSJ0cnVlIiBtc2RhdGE6TWFpbkRhdGFUYWJsZT0iVW5Lbm93IiBtc2RhdGE6VXNlQ3VycmVudExvY2FsZT0idHJ1ZSI+DQogICAgPHhzOmNvbXBsZXhUeXBlPg0KICAgICAgPHhzOmNob2ljZSBtaW5PY2N1cnM9IjAiIG1heE9jY3Vycz0idW5ib3VuZGVkIiAvPg0KICAgIDwveHM6Y29tcGxleFR5cGU+DQogIDwveHM6ZWxlbWVudD4NCjwveHM6c2NoZW1hPgYFAAAAgAE8ZGlmZmdyOmRpZmZncmFtIHhtbG5zOm1zZGF0YT0idXJuOnNjaGVtYXMtbWljcm9zb2Z0LWNvbTp4bWwtbXNkYXRhIiB4bWxuczpkaWZmZ3I9InVybjpzY2hlbWFzLW1pY3Jvc29mdC1jb206eG1sLWRpZmZncmFtLXYxIiAvPgQDAAAADlN5c3RlbS5WZXJzaW9uBAAAAAZfTWFqb3IGX01pbm9yBl9CdWlsZAlfUmV2aXNpb24AAAAACAgICAIAAAAAAAAA//////////8L7gEAAAABAAAA/////wEAAAAAAAAABAEAAAB/U3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tTeXN0ZW0uT2JqZWN0LCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQMAAAAGX2l0ZW1zBV9zaXplCF92ZXJzaW9uBQAACAgJAgAAABsAAAAbAAAAEAIAAAAgAAAABgMAAAAGMjAyMDA3BgQAAAAGMjAyMDA3CAUFMTUuMDUGBQAAAAYyMDIwMDgGBgAAAAYyMDIwMDgIBQUxNC44NwYHAAAABjIwMjAwOQYIAAAABjIwMjAwOQgFBTE0LjU4BgkAAAAGMjAyMDEwBgoAAAAGMjAyMDEwCAUFMTQuNDIGCwAAAAYyMDIwMTEGDAAAAAYyMDIwMTEIBQUxNS4zNQYNAAAABjIwMjAxMgYOAAAABjIwMjAxMggFBTE2LjE0Bg8AAAAGMjAyMTAxBhAAAAAGMjAyMTAxCAUFMTYuMzYGEQAAAAYyMDIxMDIGEgAAAAYyMDIxMDIIBQUxNy4xNAYTAAAABjIwMjEwMwYUAAAABjIwMjEwMwgFBTE3LjY3DQUL"
}
```

2. �إߤ@�Ӧۭq����A�ȨϥΩ�椸���ժ�������
	- �������󥲶����H�U�\��    
		- ����]�w�����^���ɶ�
    	- ����]�w�̤j�P�ɽШD�`��, �W�X�̤j�ШD�`�ƮɽЩߥX�ҥ~
    	- ��ܽШD�B�z�ƶq

3. �إߤ@�ӥi�H�N�ШD�H���������T�x�ۭq����D�������O
	- �����O����������ϥΪ̦ۦ�N���W����@���u��HTTP�ۭq������P������άO��L�ۭq�����@
4. �إߤ@�ӯ������U�x�D���̤j�ШD�ƶq�����O
	- ��Ҧ��D�����m�ɷ|�H����ܤ@�x����, ���L�ɫh�����n��D�P�B������, ���즳���m���D�����u���N�ШD�������Ŷ����D��