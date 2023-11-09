# Centor Criteria
The Centor Criteria is a four-point scoring system, created by Dr. Robert M. Centor, used to assist physicians in estimating the probability that pharyngitis is streptococcal (i.e. strep throat), based on five criteria:
<ul>
  <li>Age</li>
  <li>Swolen Tonsils</li>
  <li>Swolen Lymph Nodes</li>
  <li>Temperature</li>
  <li>Presence of Cough</li>
</ul>

This code is a serverless Azure Function that takes a SymptomModel (containing the symptoms, above) and returns a CentorResultModel containing a confidence interval prediction and recommended course of action (e.g. 28% - 35%: Consider rapid strep testing and/or culture).
<br />
<br />
I have also implemented Swagger to document the API: 

![image](https://github.com/krochkind/Centor/assets/64739529/379b5444-9f7e-467e-b3f8-9e852915d24d)
![image](https://github.com/krochkind/Centor/assets/64739529/8e009e64-ef9b-4f4a-bd43-cd981b20caef)
