using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Prometheus.Irony
{
    class PrototypeGrammar :  Grammar
    {
        public PrototypeGrammar() : base(caseSensitive: false)
        {
            #region Regular Expressions

            RegexBasedTerminal realNum = new RegexBasedTerminal("REAL", @"[0-9]+\.[0-9]+");
            RegexBasedTerminal intNum = new RegexBasedTerminal("INTEGER", @"[0-9]+");
            RegexBasedTerminal aritmethic = new RegexBasedTerminal("ARITHMETIC", @"[\+\-/*%]");
            RegexBasedTerminal comparison = new RegexBasedTerminal("COMPARISON", @"==|<>|<=|>=|<|>");
            RegexBasedTerminal logical = new RegexBasedTerminal("LOGICAL", @"&&|\|\||!");
            RegexBasedTerminal str = new RegexBasedTerminal("STRING", "\".*\"|\'.*\'");
            IdentifierTerminal id = new IdentifierTerminal("ID");

            #endregion

            #region Terminals

            CommentTerminal lineComment = new CommentTerminal("LINE-COMMENT", "//", "\n", "\r\n");
            CommentTerminal blockComment = new CommentTerminal("BLOCK-COMMENT", "/*", "*/");
            KeyTerm plus = ToTerm("+");
            KeyTerm increment = ToTerm("++");
            KeyTerm decrement = ToTerm("--");
            KeyTerm exponencial = ToTerm("**");
            KeyTerm logarithm = ToTerm("//");
            KeyTerm ternary = ToTerm("??");
            KeyTerm assign = ToTerm("=");
            KeyTerm assert = ToTerm("?=");
            KeyTerm colon = ToTerm(":");
            KeyTerm dot = ToTerm(".");
            KeyTerm comma = ToTerm(",");
            KeyTerm openBrace = ToTerm("{");
            KeyTerm closeBrace = ToTerm("}");
            KeyTerm openParenthesis = ToTerm("(");
            KeyTerm closeParenthesis = ToTerm(")");
            KeyTerm openBracket = ToTerm("[");
            KeyTerm closeBracket = ToTerm("]");
            KeyTerm arrow = ToTerm("->");
            KeyTerm useTerminal = ToTerm("use");
            KeyTerm forTerminal = ToTerm("for");
            KeyTerm ifTerminal = ToTerm("if");
            KeyTerm caseTerminal = ToTerm("case");
            KeyTerm defaultTerminal = ToTerm("default");
            KeyTerm elseTerminal = ToTerm("else");
            KeyTerm gotoTerminal = ToTerm("goto");
            KeyTerm pointTerminal = ToTerm("point");
            KeyTerm breakpointTerminal = ToTerm("breakpoint");
            KeyTerm funcTerminal = ToTerm("func");
            KeyTerm mainFunction = ToTerm("main");
            KeyTerm returnTerminal = ToTerm("return");
            KeyTerm trueTerminal = ToTerm("true");
            KeyTerm falseTerminal = ToTerm("false");
            KeyTerm nullTerminal = ToTerm("null");
            KeyTerm classTerminal = ToTerm("class");
            KeyTerm voidTerminal = ToTerm("void");
            KeyTerm varData = ToTerm("var");
            KeyTerm intData = ToTerm("int");
            KeyTerm realData = ToTerm("real");
            KeyTerm stringData = ToTerm("string");
            KeyTerm boolData = ToTerm("bool");
            KeyTerm arrayData = ToTerm("array");

            #endregion

            #region NonTerminals

            NonTerminal start = new NonTerminal("START");
            NonTerminal codeBlock = new NonTerminal("CODE-BLOCK");
            NonTerminal dataType = new NonTerminal("DATATYPE");
            NonTerminal methodDataType = new NonTerminal("METHOD-DATATYPE");
            NonTerminal number = new NonTerminal("NUMBER");
            NonTerminal boolean = new NonTerminal("BOOLEAN");
            NonTerminal import = new NonTerminal("IMPORT");
            NonTerminal library = new NonTerminal("LIBRARY");
            NonTerminal variableCreation = new NonTerminal("VARIABLE-CREATION");
            NonTerminal variableAssert = new NonTerminal("VARIABLE-ASSERT");
            NonTerminal variableNormal = new NonTerminal("VARIABLE-NORMAL");
            NonTerminal variableAsign = new NonTerminal("VARIABLE-ASIGN");
            NonTerminal value = new NonTerminal("VALUE");
            NonTerminal incDec = new NonTerminal("INCDEC");
            NonTerminal expLog = new NonTerminal("EXPLOG");
            NonTerminal incDecExpLog = new NonTerminal("INCDECEXPLOG");
            NonTerminal expression = new NonTerminal("EXPRESSION");
            NonTerminal numberExpression = new NonTerminal("NUMBER-EXPRESSION");
            NonTerminal strExpression = new NonTerminal("STR-EXPRESSION");
            NonTerminal objectExpression = new NonTerminal("OBJECT-EXPRESSION");
            NonTerminal boolExpression = new NonTerminal("BOOL-EXPRESSION");
            NonTerminal idValue = new NonTerminal("IDVALUE");
            NonTerminal listIdValue = new NonTerminal("LIST-IDVALUE");
            NonTerminal ternaryOperator = new NonTerminal("TERNARY-OPERATOR");
            NonTerminal arrayDeclaration = new NonTerminal("ARRAY-DECLARATION");
            NonTerminal controlStructure = new NonTerminal("CONTROL-STRUCTURE");
            NonTerminal ifStructure = new NonTerminal("IF-STRUCTURE");
            NonTerminal elseStructure = new NonTerminal("ELSE-STRUCTURE");
            NonTerminal boolNonTerminal = new NonTerminal("BOOL-NONTERMINAL");
            NonTerminal forStructure = new NonTerminal("FOR-STRUCTURE");
            NonTerminal caseStructure = new NonTerminal("CASE-STRUCTURE");
            NonTerminal pointStructure = new NonTerminal("POINT-STRUCTURE");
            NonTerminal gotoStructure = new NonTerminal("GOTO-STRUCTURE");
            NonTerminal classStructure = new NonTerminal("CLASS-STRUCTURE");
            NonTerminal funcStructure = new NonTerminal("FUNC-STRUCTURE");
            NonTerminal paramsList = new NonTerminal("PARAMS-LIST");
            NonTerminal mainFuncStructure = new NonTerminal("MAINFUNC-STRUCTURE");
            NonTerminal arrowStructure = new NonTerminal("ARROW-STRUCTURE");
            NonTerminal functionCodeBlock = new NonTerminal("FUNCTION-CODEBLOCK");
            NonTerminal mainCodeBlock = new NonTerminal("MAIN-CODEBLOCK");
            NonTerminal idValueExpression = new NonTerminal("IDVALUE-EXPRESSION");
            NonTerminal idValueExpressionArrow = new NonTerminal("IDVALUE-EXPRESSION-ARROW");
            NonTerminal functionCall = new NonTerminal("FUNCTION-CALL");
            NonTerminal listIdValueExpressionArrow = new NonTerminal("LIST-IDVALUE-EXPRESSION-ARROW");
            NonTerminal innerCodeBlock = new NonTerminal("INNER-CODEBLOCK");

            #endregion

            #region Production Rules

            start.Rule = this.Empty
                | import + classStructure
                | import + mainCodeBlock
                | classStructure
                | mainCodeBlock;

            import.Rule = useTerminal + library
                | useTerminal + library + import;

            library.Rule = id + dot + library
                | id;

            variableCreation.Rule = variableAssert
                | variableNormal;

            variableAssert.Rule = id + assert + idValueExpressionArrow
                | id + assert + ternaryOperator;

            variableNormal.Rule = dataType + id
                | dataType + id + assign + idValueExpressionArrow
                | dataType + id + assign + ternaryOperator;

            variableAsign.Rule = id + assign + idValueExpressionArrow
                | id + assign + ternaryOperator;

            value.Rule = number
                | str
                | boolData
                | nullTerminal;

            idValue.Rule = id
                | value;

            listIdValue.Rule = idValue
                | idValue + comma + listIdValue;

            idValueExpression.Rule = idValue
                | expression;

            idValueExpressionArrow.Rule = idValueExpression
                | arrowStructure;

            listIdValueExpressionArrow.Rule = idValueExpressionArrow
                | idValueExpressionArrow + comma + listIdValueExpressionArrow;

            number.Rule = realNum 
                | intNum;

            dataType.Rule = varData 
                | intData 
                | realData
                | stringData
                | boolData;

            methodDataType.Rule = varData
                | intData
                | realData
                | stringData
                | boolData
                | voidTerminal;

            boolean.Rule = trueTerminal
                | falseTerminal;

            numberExpression.Rule = number
                | id
                | number + aritmethic + numberExpression
                | openParenthesis + numberExpression + closeParenthesis
                | number + incDec
                | incDec + number
                | number + incDecExpLog + number;

            incDecExpLog.Rule = incDec
                | expLog;

            incDec.Rule = increment
                | decrement;

            expLog.Rule = exponencial
                | logarithm;

            strExpression.Rule = value
                | id
                | strExpression + incDec + strExpression
                | openParenthesis + strExpression + closeParenthesis
                | strExpression + plus + strExpression;

            objectExpression.Rule = id
                | id + openParenthesis + closeParenthesis
                | id + dot + objectExpression
                | id + openParenthesis + listIdValue + closeParenthesis
                | id + dot + objectExpression;

            boolExpression.Rule = id
                | trueTerminal
                | falseTerminal
                | boolNonTerminal + comparison + boolNonTerminal
                | boolNonTerminal + comparison + boolNonTerminal + logical + boolExpression;

            boolNonTerminal.Rule = idValue
                | expression
                | trueTerminal
                | falseTerminal;

            ternaryOperator.Rule = idValueExpression + ternary + idValueExpression + colon + idValueExpression;

            expression.Rule = numberExpression
                | strExpression
                | objectExpression;

            arrayDeclaration.Rule = arrayData + id
                | arrayData + id + assign + openBracket + closeBracket
                | arrayData + id + assign + openBracket + intNum + closeBracket
                | arrayData + id + assign + openBracket + id + closeBracket
                | id + assert + openBracket + closeBracket
                | id + assert + openBracket + intNum + closeBracket
                | id + assert + openBracket + id + closeBracket;

            controlStructure.Rule = ifStructure
                | forStructure;

            ifStructure.Rule = ifTerminal + idValue + openBrace + caseStructure + closeBrace
                | ifTerminal + boolExpression + openBrace + codeBlock + closeBrace
                | ifTerminal + boolExpression + openBrace + codeBlock + closeBrace + elseStructure
                | ifTerminal + idValue + openBrace + caseStructure + closeBrace + elseStructure;

            elseStructure.Rule = elseTerminal + openBrace + codeBlock + closeBrace
                | elseTerminal + ifStructure;

            caseStructure.Rule = caseTerminal + idValue + colon + codeBlock
                | caseTerminal + idValue + colon + codeBlock + caseStructure
                | defaultTerminal + colon + codeBlock;

            forStructure.Rule = forTerminal + boolExpression + openBrace + codeBlock + closeBrace
                | forTerminal + variableCreation + colon + idValue + comparison + idValue + colon + idValue + incDec + openBrace + codeBlock + closeBrace
                | forTerminal + variableCreation + colon + idValue + comparison + idValue + colon + idValue + incDec + idValue + openBrace + codeBlock + closeBrace;

            pointStructure.Rule = pointTerminal + colon + id;

            gotoStructure.Rule = gotoTerminal + colon + id;

            classStructure.Rule = classTerminal + id + openBrace + mainCodeBlock + closeBrace
                | classTerminal + id + openBrace + functionCodeBlock + closeBrace;

            mainFuncStructure.Rule = funcTerminal + mainFunction + openParenthesis + closeParenthesis + openBrace + codeBlock + closeBrace
                | mainFunction + openParenthesis + closeParenthesis + openBrace + codeBlock + closeBrace;

            funcStructure.Rule = funcTerminal + id + openParenthesis + closeParenthesis + openBrace + innerCodeBlock + closeBrace
                | funcTerminal + id + openParenthesis + paramsList + closeParenthesis + openBrace + innerCodeBlock + closeBrace
                | id + openParenthesis + paramsList + closeParenthesis + openBrace + innerCodeBlock + closeBrace;

            arrowStructure.Rule = openParenthesis + closeParenthesis + arrow + openBrace + innerCodeBlock + closeBrace
                | openParenthesis + paramsList + closeParenthesis + arrow + openBrace + innerCodeBlock + closeBrace;

            paramsList.Rule = dataType + id
                | dataType + id + comma + paramsList;

            functionCall.Rule = id + openParenthesis + closeParenthesis
                | id + openParenthesis + listIdValueExpressionArrow + closeParenthesis
                | id + openParenthesis + functionCall + closeParenthesis;

            mainCodeBlock.Rule = mainFuncStructure
                | mainFuncStructure + functionCodeBlock
                | functionCodeBlock + mainFuncStructure;

            functionCodeBlock.Rule = funcStructure
                | funcStructure + functionCodeBlock;

            innerCodeBlock.Rule = returnTerminal
                | returnTerminal + idValueExpression
                | codeBlock
                | returnTerminal + innerCodeBlock
                | returnTerminal + idValueExpression + innerCodeBlock
                | codeBlock + innerCodeBlock;

            codeBlock.Rule = this.Empty
                | variableCreation
                | variableAsign
                | arrayDeclaration
                | controlStructure
                | pointStructure
                | gotoStructure
                | functionCall
                | breakpointTerminal
                | variableCreation + codeBlock
                | variableAsign + codeBlock
                | arrayDeclaration + codeBlock
                | controlStructure + codeBlock
                | pointStructure + codeBlock
                | gotoStructure + codeBlock
                | functionCall + codeBlock
                | breakpointTerminal + codeBlock;

            #endregion

            #region Preferences
            this.Root = start;
            this.NonGrammarTerminals.Add(lineComment);
            this.NonGrammarTerminals.Add(blockComment);
            #endregion
        }
    }
}
